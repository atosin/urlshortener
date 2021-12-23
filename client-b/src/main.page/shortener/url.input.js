
import { useState } from "react";
import validator from "validator";
const UrlInput = ( { onGenerateButtonClicked, onUrlTextChanged } ) => {

    const [urlState, setUrlState] = useState({
        isValidUrl:false,
        url: ""
    });  

    const onTextChange =(e)=>{
        const isValidUrl = validator.isURL(e.target.value, { require_protocol: true,  require_valid_protocol: true });
        if(isValidUrl){
            setUrlState({isValidUrl: true, url : e.target.value})
        }else{
            setUrlState({isValidUrl: false, url : ""})
        }
        onUrlTextChanged();
    };

    const onButtonClicked =(e)=>{
        onGenerateButtonClicked(urlState.url);
    };

    return (
        <div className="form-group row my-4">
            <label className="col-sm-2 col-form-label">Enter Url</label>
            <div className="col-sm-8">
                <input className="form-control" type="url" placeholder="Enter url (with a valid protocol) to be shortened here" onChange={onTextChange}></input>
            </div>
            <button type="button" className="btn btn-primary col-sm-2" disabled={!urlState.isValidUrl} onClick={onButtonClicked}>Generate</button>
        </div>
    );
}

export default UrlInput;
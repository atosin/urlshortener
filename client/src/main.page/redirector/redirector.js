import { useEffect, useState } from "react";
import axios from "axios";
import { useParams } from "react-router-dom";

const Redirector = () => {

    const [hasSlug, setHasSlug] = useState(true);
    const [hasError, setHasError] = useState(false);

    let params = useParams();
    const SERVER_URL = process.env.REACT_APP_SERVER_URL;
    useEffect(() => {
        axios.get(`${SERVER_URL}/GetActualUrl/${params.hash}`)
          .then(function (response) {
              let result = response.data;
              if(!result){
                setHasSlug(false);
                return;
              }
              let len = result.length;
              if(len > 0 && result[len-1] !== "/"){
                result = result + '/';
              }
            window.location.href = result;

          })
          .catch(function (error) {
            setHasError(true);
            console.log(error);
          });
    });

    return (
        <form>
          {hasSlug && !hasError && <div className="col-12">Please Wait...</div>}
          {!hasSlug &&
            <div className="col-12">No matching redirect found for link.</div>}
          {hasError &&
            <div className="col-12 text-danger">Error while getting redirect url.</div>}
        </form>
    );
};

export default Redirector;
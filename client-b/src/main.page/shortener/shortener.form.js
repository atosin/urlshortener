import { useState } from "react";
import UrlInput from "./url.input";
import ShortenedUrl from "./shortenedurl.text";
import axios from "axios";

const ShortenerForm = () => {

    const [shortenedUrl, setShortenedUrl] = useState('');
    const [hasError, setHasError] = useState(false);


    const SERVER_URL = process.env.REACT_APP_SERVER_URL;

    const handleGenerateButtonClicked = (url) => {
        setShortenedUrl("Please wait");

        axios({
          method: "post",
          url: `${SERVER_URL}/GetSlugForUrl/`,
          data: url,
          headers: { "Content-Type": "application/json" },
        })
        .then(function (response) {
            let fullUrl = `${window.location.origin.toString()}/${response.data}/`;
            setShortenedUrl(fullUrl);
          })
          .catch(function (error) {
            setShortenedUrl("");
            setHasError(true);
            console.log(error);
          });
    };

    const handleUrlTextChanged =() => {
      setHasError(false);
    };

    return (
        <form>
            <UrlInput onGenerateButtonClicked={handleGenerateButtonClicked} onUrlTextChanged={handleUrlTextChanged} />
            <ShortenedUrl url={shortenedUrl} />
            {hasError && (<div className="text-danger">Error while getting shortened link.</div>)}
        </form>
    );
};

export default ShortenerForm;
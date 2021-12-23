
const ShortenedUrl = ({ url }) => (
    <div className="form-group row my-4">
        <label className="col-sm-2 col-form-label">Shortened Url</label>
        <div className="col-sm-10">
            <input className="form-control" type="text" value={url} readOnly ></input>
        </div>
    </div>
);

export default ShortenedUrl;
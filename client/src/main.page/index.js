import './main-page.css';
import Header  from './header';
import ShortenerForm from './shortener/shortener.form';
import Redirector from './redirector/redirector';
import { BrowserRouter, Routes, Route } from 'react-router-dom';

function App() {
  return (
    <div className="container">
      <Header/>
      <BrowserRouter>
    <Routes>
      <Route path="/" element={<ShortenerForm/>} />
      <Route path=":hash" element={<Redirector />} />
    </Routes>
  </BrowserRouter>
      
    </div>
  );
}

export default App;

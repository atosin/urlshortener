import React from "react";
import { render, unmountComponentAtNode } from "react-dom";
import {waitFor} from '@testing-library/react';
import { act } from "react-dom/test-utils";
import ShortenerForm from "./shortener.form";
import axios from "axios";

let container = null;

jest.mock("axios");

function dispatchInputEvent(ref, text){
  const input = document.getElementById(ref);
  let lastValue = input.value;
  input.value = text;
  let event = new Event('change', { bubbles: true });
  // hack React15
  event.simulated = true;
  let tracker = input._valueTracker;
  if (tracker) {
    tracker.setValue(lastValue);
  }
  input.dispatchEvent(event);
};

beforeEach(() => {
  // setup a DOM element as a render target
  container = document.createElement("div");
  document.body.appendChild(container);
});

afterEach(() => {
  // cleanup on exiting
  unmountComponentAtNode(container);
  container.remove();
  container = null;
});

it("Generate button is disabled if url is empty", async () => {
  act(() => {
    render(<ShortenerForm />, container);
  });

  // get a hold of the button element, and trigger some clicks on it
  const button = document.getElementById("generate-url");

  expect(button.hasAttribute('disabled')).toBe(true);

});

it("Generate button is disabled if url is invalid", async () => {
    act(() => {
      render(<ShortenerForm />, container);
    });
  
    // get a hold of the button element, and trigger some clicks on it
    const button = document.getElementById("generate-url");
    
    act(() => {
        dispatchInputEvent('url-text-input', 'www.yahoo.com');
      });

    expect(button.hasAttribute('disabled')).toBe(true);

  });

  it("Generate button is enabled if url is invalid", async () => {
    act(() => {
      render(<ShortenerForm />, container);
    });
  
    // get a hold of the button element, and trigger some clicks on it
    const button = document.getElementById("generate-url");
    
    act(() => {
        dispatchInputEvent('url-text-input', 'https://www.yahoo.com');
      });

    expect(button.hasAttribute('disabled')).toBe(false);

  });

  it("Does not makes a request if generate button not clickeable", async () => {

    axios.mockImplementation(() => Promise.resolve());

    act(() => {
      render(<ShortenerForm />, container);
    });
  
    // get a hold of the button element, and trigger some clicks on it
    const button = document.getElementById("generate-url");
    
    act(() => {
        dispatchInputEvent('url-text-input', '//www.yahoo.com');
      });

    act(() => {
        button.dispatchEvent(new MouseEvent("click", {bubbles : true}));
      });

    expect(axios).toHaveBeenCalledTimes(0);

  });

  it("Makes a request if generate button is clicked and enabled", async () => {

    axios.mockImplementation(() => Promise.resolve());

    act(() => {
      render(<ShortenerForm />, container);
    });
  
    // get a hold of the button element, and trigger some clicks on it
    const button = document.getElementById("generate-url");
    
    act(() => {
        dispatchInputEvent('url-text-input', 'https://www.yahoo.com');
      });

    act(() => {
        button.dispatchEvent(new MouseEvent("click", {bubbles : true}));
      });

    expect(axios).toHaveBeenCalled();

  });

  it("Error message is displayed if request fails", async () => {

    axios.mockImplementation(() => Promise.reject());

    act(() => {
      render(<ShortenerForm />, container);
    });
  
    // get a hold of the button element, and trigger some clicks on it
    const button = document.getElementById("generate-url");
    
    act(() => {
        dispatchInputEvent('url-text-input', 'https://www.yahoo.com');
      });

    await act(async () => {
        button.dispatchEvent(new MouseEvent("click", {bubbles : true}));
      });
    
    await waitFor(() => {
      const input = document.getElementById("shortened-url");

      expect(input.value).toBe("");
      expect(container.textContent).toContain("Error while getting shortened link");
    });

  });

  it("No Error message is displayed if request succeeds", async () => {

    axios.mockImplementation(() => Promise.resolve({data:"xygdsety"}));

    act(() => {
      render(<ShortenerForm />, container);
    });
  
    // get a hold of the button element, and trigger some clicks on it
    const button = document.getElementById("generate-url");
    
    act(() => {
        dispatchInputEvent('url-text-input', 'https://www.yahoo.com');
      });

    await act(async () => {
        button.dispatchEvent(new MouseEvent("click", {bubbles : true}));
      });
    
    await waitFor(() => {
      const input = document.getElementById("shortened-url");

      expect(input.value).toContain("xygdsety");
    }, {timeout: 500});

  });
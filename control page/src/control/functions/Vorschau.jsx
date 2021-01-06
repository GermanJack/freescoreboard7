import React, { Component } from 'react';

class Vorschau extends Component {
  constructor(props) {
    super(props);
    this.state = {
    };
  }

  getQueryVariable(variable) {
    const query = window.location.search.substring(1);
    const vars = query.split('&');
    for (let i = 0; i < vars.length; i += 1) {
      const pair = vars[i].split('=');
      if (pair[0] === variable) {
        return pair[1];
      }
    }

    return (false);
  }

  makeURL() {
    const webpage = window.location.search.split('?')[0];
    const websocketPort = this.getQueryVariable('wsp');
    const url = websocketPort ? `${webpage}/display/index.html?wsp=${websocketPort}` : '';
    return url;
  }

  render() {
    const url = this.makeURL();

    return (
      <div className="container">

        <div className="embed-responsive embed-responsive-16by9">
          <iframe
            id="Vorschau"
            src={url}
            title="Vorschau"
            width="100%"
            height="100%"
          />
        </div>
      </div>
    );
  }
}

export default Vorschau;

import React, { Component } from 'react';
import QRCode from 'qrcode.react';
import PropTypes from 'prop-types';

class Info extends Component {
  getQueryVariable(variable) {
    const query = window.location.search.substring(1);
    // console.log(query)//"app=article&act=news_content&aid=160990"
    const vars = query.split('&');
    // console.log(vars) //[ 'app=article', 'act=news_content', 'aid=160990' ]
    for (let i = 0; i < vars.length; i += 1) {
      const pair = vars[i].split('=');
      // console.log(pair)//[ 'app', 'article' ][ 'act', 'news_content' ][ 'aid', '160990' ]
      if (pair[0] === variable) {
        return pair[1];
      }
    }
    return (false);
  }

  copytext(id) {
    const copyText = document.getElementById(id);
    copyText.select();
    copyText.setSelectionRange(0, 99999);
    document.execCommand('copy');
    // alert("Copied the text: " + copyText.value);
  }

  render() {
    const wsp = this.getQueryVariable('wsp');
    const displayUri = `http://${window.location.hostname}:${window.location.port}/display/index.html?wsp=${wsp}`;
    const controlUri = `http://${window.location.hostname}:${window.location.port}/control/index.html?wsp=${wsp}`;

    return (
      <div className="App">
        <div className="border">
          <div className="text-center bg-fsc text-light p-1">
            {`Info Version ${this.props.Version}`}
          </div>
          <div className="h1 text-center">
            <a
              href="https://www.freescoreboard.org/"
              target="_blank"
              rel="noopener noreferrer"
            >
              www.freescoreboard.org
            </a>
          </div>
          <div className="p-1 m-1 text-center">
            Anregungen an:
            <p>
              <a href="mailto:info@freescoreboard.org?Subject=Anregung" target="_top">info@freescoreboard.org</a>
            </p>
          </div>

          <div className="d-flex flex-row justify-content-center">

            <div className="p-2 m-2 d-flex flex-column justify-content-center border border-dark">
              <div className="text-center h4">
                Kontrolfenster
              </div>
              <div className="d-flex justify-content-center">
                <QRCode
                  value={controlUri}
                />
              </div>
              <div id="contol">
                <a href={controlUri} target="_blank" rel="noopener noreferrer">
                  {controlUri}
                </a>
              </div>
            </div>

            <div className="p-2 m-2 d-flex flex-column justify-content-center border border-dark text-center">
              <div className="text-center h4">
                Anzeigefenster
              </div>
              <div className="d-flex justify-content-center">
                <QRCode
                  value={displayUri}
                />
              </div>
              <div id="contol">
                <a href={displayUri} target="_blank" rel="noopener noreferrer">
                  {displayUri}
                </a>
              </div>
            </div>

          </div>

          <div className="text-center">
            freeScoreBoard 7 wurde durch folgende Projekte möglich:
            <br />
            <br />
            <a href="https://reactjs.org/" target="_blank" rel="noopener noreferrer">ReactJS</a>
            <br />
            <a href="https://github.com/cefsharp/CefSharp" target="_blank" rel="noopener noreferrer">CEFSharp</a>
            <br />
            <a href="https://getbootstrap.com/" target="_blank" rel="noopener noreferrer">Bootstrap</a>
            <br />
            <a href="https://jquery.com/" target="_blank" rel="noopener noreferrer">JQuery</a>
            <br />
            <a href="https://www.npmjs.com/package/qrcode.react" target="_blank" rel="noopener noreferrer">QRCode React</a>
            <br />
            <a href="https://casesandberg.github.io/react-color/" target="_blank" rel="noopener noreferrer">React ColorPicker</a>
            <br />
            <a href="https://github.com/mgravell/fast-member" target="_blank" rel="noopener noreferrer">FastMember</a>
            <br />
            <a href="https://www.sqlite.org/index.html" target="_blank" rel="noopener noreferrer">SQLite</a>
            <br />
            <a href="https://github.com/sta/websocket-sharp" target="_blank" rel="noopener noreferrer">WebSocketSharp</a>
            <br />
            <a href="https://www.npmjs.com/package/react-rnd" target="_blank" rel="noopener noreferrer">react-rnd</a>
            <br />
            <a href="https://www.npmjs.com/package/react-ticker" target="_blank" rel="noopener noreferrer">react-ticker</a>
            <br />
            <a href="https://github.com/gregnb/react-to-print" target="_blank" rel="noopener noreferrer">react-to-print</a>
            <br />
            <a href="https://www.npmjs.com/package/graphviz-react" target="_blank" rel="noopener noreferrer">graphviz-react</a>
            <br />
            <br />
            Danke an alle Freeware Entwickler!
            <br />
            Ich hoffe ich habe niemanden vergessen.
            <br />
          </div>

        </div>
      </div>
    );
  }
}

Info.propTypes = {
  Version: PropTypes.string,
};

Info.defaultProps = {
  Version: '0',
};

export default Info;

import React, { Component } from 'react';
import PropTypes from 'prop-types';

class IFrame extends Component {
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

  makeDesignerURL() {
    const webpage = window.location.search.split('?')[0];
    // const webpage = window.location.host;
    const websocketPort = this.getQueryVariable('wsp');
    const designerurl = websocketPort
      ? `${webpage}/display/index.html?wsp=${websocketPort}&pageset=${this.props.profil}&page=${this.props.page}&divid=${this.props.objektid}` : '';
    // eslint-disable-next-line no-console
    console.log(`designerurl: ${designerurl}`);
    return designerurl;
  }

  render() {
    const designerurl = this.makeDesignerURL();
    // console.log('desingerurl: ' + designerurl);
    let ifr = null;
    if (!designerurl.includes('/.html')) {
      ifr = (
        <iframe
          key={this.props.refresh.toString()}
          id="iframe"
          className="embed-responsive-item p-2"
          title="page"
          // ref="iframe"
          src={designerurl}
          allowFullScreen
        />
      );
    }
    if (this.props.link !== '') {
      ifr = (
        <iframe
          id="iframe"
          style={{
            maxheigt: '95vh',
            width: '100%',
            height: '100%',
            overflow: 'visible',
          }}
          // ref="iframe"
          src={designerurl}
          width="100%"
          height="100%"
          scrolling="yes"
          frameBorder="0"
          title="display"
        />
      );
    } else {
      ifr = <div>leer</div>;
    }

    return (
      <div style={{
        maxheigt: '95vh',
        width: '100%',
        height: '70vh',
        overflow: 'visible',
      }}
      >
        {ifr}
      </div>
    );
  }
}

IFrame.propTypes = {
  profil: PropTypes.string.isRequired,
  page: PropTypes.string.isRequired,
  objektid: PropTypes.string.isRequired,
  link: PropTypes.string.isRequired,
  refresh: PropTypes.func.isRequired,
};

export default IFrame;

import React, { Component } from 'react';
import PropTypes from 'prop-types';

class IFrame extends Component {
  constructor(props) {
    super(props);
    this.state = {
      // ratio: 'embed-responsive embed-responsive-19by9',
      // w: 800,
      // h: 600,
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
    const websocketPort = this.getQueryVariable('wsp');
    const designerurl = websocketPort ? `${webpage}/designs/${this.props.profil}/${this.props.page}.html?wsp=${websocketPort}&pageset=${this.props.profil}&page=${this.props.page}&divid=${this.props.objektid}` : '';
    return designerurl;
  }

  render() {
    const designerurl = this.makeDesignerURL();
    let ifr = null;
    if (!designerurl.includes('/.html')) {
      ifr = (
        <iframe
          key={this.props.id}
          id={this.props.id}
          className="embed-responsive-item p-2"
          title="page"
          ref={this.props.id}
          src={designerurl}
          allowFullScreen
        />
      );
    }
    if (this.props.link !== '') {
      ifr = (
        <iframe
          id={this.props.id}
          style={{
            maxheigt: '95vh',
            width: '100%',
            height: '100%',
            overflow: 'visible',
          }}
          ref={this.props.id}
          src={designerurl}
          width="100%"
          height="100%"
          scrolling="yes"
          frameBorder="0"
          title="display"
          onLoad={this.props.iframeLoaded}
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
        {/* <div>{designerurl}</div> */}
        {ifr}
      </div>
    );
  }
}

IFrame.propTypes = {
  profil: PropTypes.string,
  page: PropTypes.string,
  id: PropTypes.string,
  objektid: PropTypes.string,
  link: PropTypes.string,
  iframeLoaded: PropTypes.func,
};

IFrame.defaultProps = {
  profil: '',
  page: '',
  id: '',
  objektid: '',
  link: '',
  iframeLoaded: () => {},
};

export default IFrame;

import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { FaRegStopCircle } from 'react-icons/fa';

class Tonwahl extends Component {
  constructor(props) {
    super(props);
    this.Name = {
      value: null,
    };
    // This binding is necessary to make `this` work in the callback
    // this.constructor.handleClick = this.handleClick.bind(this);
  }

  // handleClick(funcID) {
  //   if (navigator.userAgent === 'CEF') {
  //     window.nativeHost.click(funcID);
  //     const log = document.getElementById('divlog');
  //     log.innerText += `\r\n ${funcID}`;
  //   }
  // }

  handleClick(e) {
    this.props.websocketSend({ Type: 'bef', Command: 'PlayAudio', Value1: e });
  }

  render() {
    return (
      <div className="ml-2">

        <div className="d-flex flex-row">
          <div className="d-flex flex-col">
            <div className="mr-1 bottom-group-label">
              <div className="btn-group w-100">
                <span className="input-group-text border-dark bg-light" data-labelid="BoxToene">Ton abspielen:</span>
              </div>
            </div>
          </div>
          <div className="d-flex flex-col">
            <div className="btn-toolbar">
              <div className="btn-group mb-1">
                <button type="button" className="btn btn-primary border-dark" data-labelid="BtnTon1" onClick={this.handleClick.bind(this, '1')}>Ton 1</button>
                <button type="button" className="btn btn-primary border-dark" data-labelid="BtnTon2" onClick={this.handleClick.bind(this, '2')}>Ton 2</button>
                <button type="button" className="btn btn-primary border-dark" data-labelid="BtnTon3" onClick={this.handleClick.bind(this, '3')}>Ton 3</button>
                <button type="button" className="btn btn-primary border-dark" data-labelid="BtnTon4" onClick={this.handleClick.bind(this, '4')}>Ton 4</button>
                <button type="button" className="btn btn-primary border-dark" data-labelid="BtnTon5" onClick={this.handleClick.bind(this, '5')}>Ton 5</button>
                <button type="button" className="btn btn-primary border-dark" data-labelid="BtnTon6" onClick={this.handleClick.bind(this, '6')}>Ton 6</button>
                <button type="button" className="btn btn-primary border-dark" data-labelid="BtnTon7" onClick={this.handleClick.bind(this, '7')}>Ton 7</button>
                <button type="button" className="btn btn-primary border-dark" data-labelid="BtnTon8" onClick={this.handleClick.bind(this, '8')}>Ton 8</button>
                <button type="button" className="btn btn-primary border-dark" data-labelid="BtnTonAus" onClick={this.handleClick.bind(this, '0')} label="Stop"><FaRegStopCircle /></button>
              </div>
            </div>
          </div>
        </div>
      </div>
    );
  }
}

Tonwahl.propTypes = {
  websocketSend: PropTypes.func.isRequired,
};

export default Tonwahl;

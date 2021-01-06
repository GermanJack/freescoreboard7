import React, { Component } from 'react';
import PropTypes from 'prop-types';

class Bildwahl extends Component {
  constructor(props) {
    super(props);
    this.Name = {
      value: null,
    };

    // This binding is necessary to make `this` work in the callback
    this.handleClick = this.handleClick.bind(this);
  }

  handleClick(e) {
    this.props.websocketSend({ Type: 'bef', Command: 'SetB05', Value1: e });
  }

  render() {
    return (
      <div className="ml-2">

        <div className="d-flex flex-row">
          <div className="d-flex flex-col">
            <div className="mr-1 bottom-group-label">
              <div className="btn-group w-100">
                <span className="input-group-text border-dark bg-light" data-labelid="BoxBilder">Bild einstellen:</span>
              </div>
            </div>
          </div>
          <div className="d-flex flex-col">
            <div className="btn-toolbar">
              <div className="btn-group mb-1">
                <button type="button" className="btn btn-primary border-dark" data-labelid="BtnBild1" onClick={this.handleClick.bind(this, '1')}>Bild 1</button>
                <button type="button" className="btn btn-primary border-dark" data-labelid="BtnBild2" onClick={this.handleClick.bind(this, '2')}>Bild 2</button>
                <button type="button" className="btn btn-primary border-dark" data-labelid="BtnBild3" onClick={this.handleClick.bind(this, '3')}>Bild 3</button>
                <button type="button" className="btn btn-primary border-dark" data-labelid="BtnBild4" onClick={this.handleClick.bind(this, '4')}>Bild 4</button>
                <button type="button" className="btn btn-primary border-dark" data-labelid="BtnBild5" onClick={this.handleClick.bind(this, '5')}>Bild 5</button>
                <button type="button" className="btn btn-primary border-dark" data-labelid="BtnBild6" onClick={this.handleClick.bind(this, '6')}>Bild 6</button>
                <button type="button" className="btn btn-primary border-dark" data-labelid="BtnBild7" onClick={this.handleClick.bind(this, '7')}>Bild 7</button>
                <button type="button" className="btn btn-primary border-dark" data-labelid="BtnBild8" onClick={this.handleClick.bind(this, '8')}>Bild 8</button>
              </div>
            </div>
          </div>
        </div>
      </div>
    );
  }
}

Bildwahl.propTypes = {
  websocketSend: PropTypes.func.isRequired,
};

export default Bildwahl;

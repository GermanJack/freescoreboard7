import React, { Component } from 'react';
import PropTypes from 'prop-types';

class TonwahlWin extends Component {
  constructor(props) {
    super(props);
    this.state = {
    };
  }

  getPropValue(prop) {
    const x = this.props.options.find((y) => y.Prop === prop);
    return x ? x.Value : '';
  }

  handleClick(e) {
    this.props.websocketSend({ Type: 'bef', Command: 'PlayAudio', Value1: e });
  }

  reset() {
    this.props.websocketSend({ Type: 'bef', Command: 'PlayAudio', Value1: 0 });
  }

  render() {
    return (
      <div className="">
        <div className="d-flex flex-row mb-1 justify-content-around">
          <button
            className="btn btn-outline-secondary"
            type="button"
            title={this.getPropValue('Ton1')}
            onClick={this.handleClick.bind(this, '1')}
          >
            Ton1
          </button>
          <button
            className="btn btn-outline-secondary"
            type="button"
            title={this.getPropValue('Ton2')}
            onClick={this.handleClick.bind(this, '2')}
          >
            Ton2
          </button>
          <button
            className="btn btn-outline-secondary"
            type="button"
            title={this.getPropValue('Ton3')}
            onClick={this.handleClick.bind(this, '3')}
          >
            Ton3
          </button>
          <button
            className="btn btn-outline-secondary"
            type="button"
            title={this.getPropValue('Ton4')}
            onClick={this.handleClick.bind(this, '4')}
          >
            Ton4
          </button>
        </div>
        <div className="d-flex flex-row justify-content-around">
          <button
            className="btn btn-outline-secondary"
            type="button"
            title={this.getPropValue('Ton5')}
            onClick={this.handleClick.bind(this, '5')}
          >
            Ton5
          </button>
          <button
            className="btn btn-outline-secondary"
            type="button"
            title={this.getPropValue('Ton6')}
            onClick={this.handleClick.bind(this, '6')}
          >
            Ton6
          </button>
          <button
            className="btn btn-outline-secondary"
            type="button"
            title={this.getPropValue('Ton7')}
            onClick={this.handleClick.bind(this, '7')}
          >
            Ton7
          </button>
          <button
            className="btn btn-outline-secondary"
            type="button"
            title={this.getPropValue('Ton8')}
            onClick={this.handleClick.bind(this, '8')}
          >
            Ton8
          </button>
        </div>
      </div>
    );
  }
}

TonwahlWin.propTypes = {
  options: PropTypes.arrayOf(PropTypes.object).isRequired,
  websocketSend: PropTypes.func.isRequired,
};

export default TonwahlWin;

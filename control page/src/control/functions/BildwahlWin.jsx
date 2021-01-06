import React, { Component } from 'react';
import PropTypes from 'prop-types';

class BildwahlWin extends Component {
  constructor(props) {
    super(props);
    this.state = {
      size: '2rem',
    };
  }

  getPropValue(prop) {
    const x = this.props.options.find((y) => y.Prop === prop);
    return x ? x.Value : '';
  }

  handleClick(e) {
    this.props.websocketSend({ Type: 'bef', Command: 'SetB05', Value1: e });
  }

  reset() {
  }

  render() {
    return (
      <div className="">
        <div className="d-flex flex-row mb-1 justify-content-around">
          <button
            className="btn btn-outline-secondary"
            type="button"
            onClick={this.handleClick.bind(this, '1')}
          >
            <img
              src={`./../../pictures/${this.getPropValue('SlideshowBild1')}`}
              alt=""
              style={{ maxHeight: this.state.size, maxWidth: this.state.size, align: 'middle' }}
            />
          </button>
          <button
            className="btn btn-outline-secondary"
            type="button"
            onClick={this.handleClick.bind(this, '2')}
          >
            <img
              src={`./../../pictures/${this.getPropValue('SlideshowBild2')}`}
              alt=""
              style={{ maxHeight: this.state.size, maxWidth: this.state.size, align: 'middle' }}
            />
          </button>
          <button
            className="btn btn-outline-secondary"
            type="button"
            onClick={this.handleClick.bind(this, '3')}
          >
            <img
              src={`./../../pictures/${this.getPropValue('SlideshowBild3')}`}
              alt=""
              style={{ maxHeight: this.state.size, maxWidth: this.state.size, align: 'middle' }}
            />
          </button>
          <button
            className="btn btn-outline-secondary"
            type="button"
            onClick={this.handleClick.bind(this, '4')}
          >
            <img
              src={`./../../pictures/${this.getPropValue('SlideshowBild4')}`}
              alt=""
              style={{ maxHeight: this.state.size, maxWidth: this.state.size, align: 'middle' }}
            />
          </button>
        </div>
        <div className="d-flex flex-row justify-content-around">
          <button
            className="btn btn-outline-secondary"
            type="button"
            onClick={this.handleClick.bind(this, '5')}
          >
            <img
              src={`./../../pictures/${this.getPropValue('SlideshowBild5')}`}
              alt=""
              style={{ maxHeight: this.state.size, maxWidth: this.state.size, align: 'middle' }}
            />
          </button>
          <button
            className="btn btn-outline-secondary"
            type="button"
            onClick={this.handleClick.bind(this, '6')}
          >
            <img
              src={`./../../pictures/${this.getPropValue('SlideshowBild6')}`}
              alt=""
              style={{ maxHeight: this.state.size, maxWidth: this.state.size, align: 'middle' }}
            />
          </button>
          <button
            className="btn btn-outline-secondary"
            type="button"
            onClick={this.handleClick.bind(this, '7')}
          >
            <img
              src={`./../../pictures/${this.getPropValue('SlideshowBild7')}`}
              alt=""
              style={{ maxHeight: this.state.size, maxWidth: this.state.size, align: 'middle' }}
            />
          </button>
          <button
            className="btn btn-outline-secondary"
            type="button"
            onClick={this.handleClick.bind(this, '8')}
          >
            <img
              src={`./../../pictures/${this.getPropValue('SlideshowBild8')}`}
              alt=""
              style={{ maxHeight: this.state.size, maxWidth: this.state.size, align: 'middle' }}
            />
          </button>
        </div>
      </div>
    );
  }
}

BildwahlWin.propTypes = {
  options: PropTypes.arrayOf(PropTypes.object).isRequired,
  websocketSend: PropTypes.func.isRequired,
};

export default BildwahlWin;

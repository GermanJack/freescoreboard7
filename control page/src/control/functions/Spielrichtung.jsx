import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { FaArrowLeft, FaTimes, FaArrowRight } from 'react-icons/fa';

class Spielrichtung extends Component {
  constructor(props) {
    super(props);
    this.state = {
      sel: 'x',
    };
  }

  reset() {
    this.props.websocketSend({
      Domain: 'KO', Type: 'bef', Command: 'ResetSpielrichtung',
    });
  }

  click(e) {
    this.props.websocketSend({
      Domain: 'KO', Type: 'bef', Command: 'SetSpielrichtung', Value1: e,
    });
    this.setState({ sel: e });
  }

  render() {
    const classActive = 'btn btn-success border border-dark p-1';
    const classDeActive = 'btn btn-primary border border-dark p-1';

    return (
      <div className="d-flex justify-content-around">
        <button
          type="button"
          className={this.state.sel === 'l' ? classActive : classDeActive}
          onClick={this.click.bind(this, 'l')}
        >
          <FaArrowLeft size="1.5em" />
        </button>
        <button
          type="button"
          className={this.state.sel === 'x' ? classActive : classDeActive}
          onClick={this.click.bind(this, 'x')}
        >
          <FaTimes size="1.5em" />
        </button>
        <button
          type="button"
          className={this.state.sel === 'r' ? classActive : classDeActive}
          onClick={this.click.bind(this, 'r')}
        >
          <FaArrowRight size="1.5em" />
        </button>
      </div>
    );
  }
}

Spielrichtung.propTypes = {
  websocketSend: PropTypes.func.isRequired,
};

export default Spielrichtung;

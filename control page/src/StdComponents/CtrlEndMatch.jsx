/* eslint-disable jsx-a11y/label-has-associated-control */
import React, { Component } from 'react';
import PropTypes from 'prop-types';

class CtrlEndMatch extends Component {
  constructor(props) {
    super(props);
    this.state = {
    };
  }

  valueChange(e) {
    this.props.CtrlEndMatchChange(e);
  }

  render() {
    const value = this.props.SpielEndeOption;
    let value1 = false;
    let value2 = false;
    let value3 = false;

    if (value === '1') {
      value1 = true;
    }
    if (value === '2') {
      value2 = true;
    }
    if (value === '3') {
      value3 = true;
    }
    return (
      <div className="">
        <div className="form-check">
          <input
            className="form-check-input"
            type="radio"
            name="Radios"
            id="Radios1"
            value="1"
            checked={value1}
            onChange={this.valueChange.bind(this)}
          />
          <label className="form-check-label" htmlFor="Radios1">
            Spiel Schließen
          </label>
        </div>
        <div className="form-check">
          <input
            className="form-check-input"
            type="radio"
            name="Radios"
            id="Radios2"
            value="2"
            checked={value2}
            onChange={this.valueChange.bind(this)}
          />
          <label className="form-check-label" htmlFor="Radios2">
            Spiel offen lassen
          </label>
        </div>
        <div className="form-check">
          <input
            className="form-check-input"
            type="radio"
            name="Radios"
            id="Radios3"
            value="3"
            checked={value3}
            onChange={this.valueChange.bind(this)}
          />
          <label className="form-check-label" htmlFor="Radios3">
            Spiel Schließen und beiden Mannschaften 5 Gegentore werten (Beide nicht angetreten)
          </label>
        </div>
      </div>
    );
  }
}

CtrlEndMatch.propTypes = {
  SpielEndeOption: PropTypes.string.isRequired,
  CtrlEndMatchChange: PropTypes.func.isRequired,
};

CtrlEndMatch.defaultProps = {
};

export default CtrlEndMatch;

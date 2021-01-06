/* eslint-disable import/no-dynamic-require */
import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { GoPlus, GoDash } from 'react-icons/go';
import { MdSentimentSatisfied, MdSentimentDissatisfied } from 'react-icons/md';

class Pic5Counter extends Component {
  handleClick(funcID) {
    if (funcID === '+' && this.props.count === 5) {
      return;
    }

    this.props.click(this.props.variable, funcID);
  }

  render() {
    const live = <div className="flex-fill border border-dark border-right-0 bg-good text-dark pl-1 pr-1"><MdSentimentSatisfied /></div>;
    const dead = <div className="flex-fill border border-dark border-right-0 bg-danger text-dark pl-1 pr-1"><MdSentimentDissatisfied /></div>;

    return (
      <div className="d-flex justify-content-around">
        <div className="container p-0">
          <div className="d-flex flex-row p-0">
            <button
              type="button"
              className="btn btn-warning btn-sm border-dark btn-font-2 p-1"
              onClick={this.handleClick.bind(this, '-')}
            >
              <GoDash size="1.5em" color="black" />
            </button>
            {this.props.count > 0 ? live : dead}
            {this.props.count > 1 ? live : dead}
            {this.props.count > 2 ? live : dead}
            {this.props.count > 3 ? live : dead}
            {this.props.count > 4 ? live : dead}
            <button
              type="button"
              className="btn btn-success btn-sm border-dark btn-font-2 p-1"
              onClick={this.handleClick.bind(this, '+')}
            >
              <GoPlus size="1.5em" color="white" />
            </button>
          </div>
        </div>
      </div>
    );
  }
}

Pic5Counter.propTypes = {
  count: PropTypes.number.isRequired,
  variable: PropTypes.string.isRequired,
  click: PropTypes.func.isRequired,
};

export default Pic5Counter;

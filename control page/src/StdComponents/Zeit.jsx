import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { FaRegPlayCircle, FaRegStopCircle } from 'react-icons/fa';
import { GiBackwardTime } from 'react-icons/gi';

import DlgTime from './DlgTime';
import DlgWarning from './DlgWarning';

class Zeit extends Component {
  constructor(props) {
    super(props);
    this.state = {
    };

    // This binding is necessary to make `this` work in the callback
    this.handleClick = this.handleClick.bind(this);
  }

  handleClick(funcID) {
    this.props.click(funcID, this.props.timer.Nr);
  }

  send(e) {
    this.props.websocketSend({
      Domain: 'KO',
      Type: 'bef',
      Command: 'TimerManipulate',
      Value1: this.props.timer.Nr,
      Value2: e,
    });
  }

  render() {
    const start = <FaRegPlayCircle />;
    const stop = <FaRegStopCircle />;
    const reset = <GiBackwardTime />;
    let displaymod = 'sec';
    if (this.props.timer.MinutenDarstellung) {
      displaymod = 'min';
    }

    let knopf = (
      <button
        type="button"
        className="btn btn-success border-dark pt-0 pb-0 pl-3 pr-3"
        onClick={this.handleClick.bind(this, 'TimerOn')}
      >
        {start}
      </button>
    );
    if (this.props.active) {
      knopf = (
        <button
          type="button"
          className="btn btn-warning order-dark pt-0 pb-0 pl-3 pr-3"
          onClick={this.handleClick.bind(this, 'TimerOff')}
        >
          {stop}
        </button>
      );
    }

    return (
      <div className="mb-1 container p-0">
        <div className="row p-0 m-0">

          <div className="col pr-1 p-0 m-0">
            <span className="input-group-text border-dark w-100 bg-light">{this.props.timer.Name}</span>
          </div>

          <div className="col pr-1 p-0 m-0">
            <span className="input-group-text border-dark w-100 justify-content-center bg-light">{this.props.wert}</span>
          </div>

          <div className="col p-0 m-0">
            <div className="row p-0 m-0">
              <div className="btn-group">
                {knopf}
                {/* <button
                  type="button"
                  className="btn btn-primary border-dark pl-3 pr-3"
                  onClick={this.handleClick.bind(this, 'TimerOn')}
                >
                  {start}
                </button>

                <button
                  type="button"
                  className="btn btn-primary border-dark pl-3 pr-3"
                  onClick={this.handleClick.bind(this, 'TimerOff')}
                >
                  {stop}
                </button> */}

                <DlgWarning
                  label1={`${this.props.timer.Name} wirklich resetten?`}
                  btnclassname="btn btn-primary border-dark ml-1"
                  reacticon={reset}
                  iconAltText="r"
                  toolTip="reset"
                  modalID={`Modal-time-reset${this.props.timer.Nr}`}
                  name={this.handleClick.bind(this, 'TimerReset')}
                />

                <DlgTime
                  btnclassname="btn btn-primary border-dark ml-1"
                  wert={this.props.wert}
                  modalID={`edit${this.props.timer.Variable}`}
                  displaymod={displaymod}
                  label1={`${this.props.timer.Name} ändern:`}
                  toolTip={`${this.props.timer.Name} ändern:`}
                  newTime={this.send.bind(this)}
                  disabled={false}
                />
              </div>
            </div>
          </div>

        </div>

      </div>
    );
  }
}

Zeit.propTypes = {
  timer: PropTypes.arrayOf(PropTypes.object),
  active: PropTypes.bool,
  wert: PropTypes.string,
  click: PropTypes.func,
  websocketSend: PropTypes.func,
};

Zeit.defaultProps = {
  timer: [],
  active: false,
  wert: '',
  click: () => {},
  websocketSend: () => {},
};

export default Zeit;

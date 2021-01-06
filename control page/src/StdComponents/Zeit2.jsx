import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { FaRegPlayCircle, FaRegStopCircle } from 'react-icons/fa';

import DlgTime from './DlgTime';

class Zeit2 extends Component {
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
    const start = <FaRegPlayCircle size="1.5em" />;
    const stop = <FaRegStopCircle size="1.5em" />;

    let displaymod = 'sec';
    if (this.props.timer.MinutenDarstellung) {
      displaymod = 'min';
    }

    let knopf = (
      <button
        type="button"
        className="btn btn-success border-dark pl-3 pr-3"
        onClick={this.handleClick.bind(this, 'TimerOn')}
      >
        {start}
      </button>
    );
    if (this.props.active) {
      knopf = (
        <button
          type="button"
          className="btn btn-warning border-dark pl-3 pr-3"
          onClick={this.handleClick.bind(this, 'TimerOff')}
        >
          {stop}
        </button>
      );
    }

    return (
      <div className="p-0">
        <div className="row p-0 m-0">

          <div className="border border-dark rounded text-center bg-light p-0 m-0 mr-1">
            <div className=" m-0 pl-4 pr-4" style={{ fontSize: '1.5rem' }}>
              {this.props.wert}
            </div>
          </div>

          {knopf}

          <DlgTime
            btnclassname="btn btn-primary border-dark ml-5 pl-1 pr-1"
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
    );
  }
}

Zeit2.propTypes = {
  timer: PropTypes.arrayOf(PropTypes.object),
  active: PropTypes.bool,
  wert: PropTypes.string,
  click: PropTypes.func,
  websocketSend: PropTypes.func,
};

Zeit2.defaultProps = {
  active: false,
  timer: [],
  wert: '',
  click: () => {},
  websocketSend: () => {},
};

export default Zeit2;

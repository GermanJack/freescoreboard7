import React, { Component } from 'react';
import PropTypes from 'prop-types';

class MinuteTime extends Component {
  constructor(props) {
    super(props);
    this.state = {
    };
  }

  setPropValue(t, e) {
    let snew = 0;
    const m = Math.trunc(this.props.seconds / 60);
    const s = this.props.seconds - (m * 60);

    const eing = parseInt(e.target.value, 10);

    if (t === 'm') {
      snew = (eing * 60) + s;
    }

    if (t === 's') {
      snew = (m * 60) + eing;
    }

    if (Number.isNaN(snew)) {
      snew = 0;
    }

    this.props.TimeSet(snew);
  }

  render() {
    const m = Math.trunc(this.props.seconds / 60);
    const s = this.props.seconds - (m * 60);

    return (
      <div className="d-flex flex-row mt-1">
        <input
          title="Minuten"
          type="number"
          className="form-control mb-1 mr-2"
          style={{ width: '75px' }}
          min="0"
          max="100000"
          value={m}
          onChange={this.setPropValue.bind(this, 'm')}
        />

        <div>:</div>

        <input
          title="Sekunden"
          type="number"
          className="form-control mb-1 ml-2 mr-2"
          style={{ width: '75px' }}
          min="0"
          max="59"
          value={s}
          onChange={this.setPropValue.bind(this, 's')}
        />
      </div>
    );
  }
}

MinuteTime.propTypes = {
  seconds: PropTypes.number,
  TimeSet: PropTypes.func,
};

MinuteTime.defaultProps = {
  seconds: 0,
  TimeSet: () => {},
};

export default MinuteTime;

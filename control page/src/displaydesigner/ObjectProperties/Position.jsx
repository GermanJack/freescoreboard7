import React, { Component } from 'react';
import PropTypes from 'prop-types';

class Position extends Component {
  constructor(props) {
    super(props);
    this.state = {
    };
  }

  onTopChange(e) {
    const value = `${e.target.value}vh`;
    this.props.onStyleChange('top', value);
  }

  onLeftChange(e) {
    const value = `${e.target.value}vw`;
    this.props.onStyleChange('left', value);
  }

  render() {
    const o = this.props.top ? parseFloat(this.props.top) : 0;
    const l = this.props.left ? parseFloat(this.props.left) : 0;
    return (
      <div className="border border-dark">
        <div className="text-white bg-secondary pl-1 pr-1">Position</div>
        <div className="p-1">

          <div className="d-flex flex-row pb-1">
            <div className="input-group p-0">
              <div className="input-group-prepend m-0">
                <div className="input-group-text p-0 pl-1 pr-1" style={{ width: '20px' }}>O</div>
              </div>
              <input
                title="Abstand von Oben in % der Anzeigehöhe"
                type="number"
                className=""
                style={{ width: '60px' }}
                min="0"
                max="95"
                value={o}
                onChange={this.onTopChange.bind(this)}
              />
            </div>
          </div>

          <div className="d-flex flex-row">
            <div className="input-group p-0">
              <div className="input-group-prepend m-0">
                <div className="input-group-text p-0 pl-1 pr-1" style={{ width: '20px' }}>L</div>
              </div>
              <input
                title="Abstand on Links in % der Anzeigebreite"
                type="number"
                className=""
                style={{ width: '60px' }}
                min="0"
                max="95"
                value={l}
                onChange={this.onLeftChange.bind(this)}
              />
            </div>
          </div>

        </div>
      </div>
    );
  }
}

Position.propTypes = {
  top: PropTypes.string.isRequired,
  left: PropTypes.string.isRequired,
  onStyleChange: PropTypes.func.isRequired,
};

export default Position;

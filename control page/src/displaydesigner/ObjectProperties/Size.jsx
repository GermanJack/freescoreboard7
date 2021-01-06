/* eslint-disable jsx-a11y/label-has-associated-control */
import React, { Component } from 'react';
import PropTypes from 'prop-types';

class Size extends Component {
  constructor(props) {
    super(props);
    this.state = {
    };
  }

  onHeightChange(e) {
    const value = `${e.target.value}vh`;
    this.props.onStyleChange('height', value);
  }

  onWidthChange(e) {
    const value = `${e.target.value}vw`;
    this.props.onStyleChange('width', value);
  }

  onVisibleChange(e) {
    const value = e.target.checked;
    let value1 = 'hidden';
    if (value === true) {
      value1 = 'visible';
    }

    this.props.onStyleChange('visibility', value1);
  }

  checkVisibility(e) {
    let value = false;
    if (e === 'visible') {
      value = true;
    }
    return value;
  }

  render() {
    const h = this.props.height ? parseFloat(this.props.height) : 0;
    const w = this.props.width ? parseFloat(this.props.width) : 0;
    const v = this.checkVisibility(this.props.visibility);
    return (
      <div className="border border-dark">
        <div className="text-white bg-secondary pl-1 pr-1">Größe</div>
        <div className="p-1">

          <div className="d-flex flex-row pb-1">
            <div className="input-group p-0">
              <div className="input-group-prepend m-0">
                <div className="input-group-text p-0 pl-1 pr-1" style={{ width: '20px' }}>Y</div>
              </div>
              <input
                title="Höhe in % der Anzeigehöhe"
                type="number"
                className=""
                style={{ width: '60px' }}
                min="0"
                max="100"
                value={h}
                onChange={this.onHeightChange.bind(this)}
              />
            </div>
          </div>

          <div className="d-flex flex-row">
            <div className="input-group p-0">
              <div className="input-group-prepend m-0">
                <div className="input-group-text p-0 pl-1 pr-1" style={{ width: '20px' }}>X</div>
              </div>
              <input
                title="Breite in % der Anzeigebreite"
                type="number"
                className=""
                style={{ width: '60px' }}
                min="0"
                max="100"
                value={w}
                onChange={this.onWidthChange.bind(this)}
              />
            </div>
          </div>

          <div className="d-flex flex-row">
            <input
              title="Ist das Objekt sichtbar?"
              type="checkbox"
              className="mt-2 ml-2 mr-2"
              checked={v}
              id="visibility"
              onChange={this.onVisibleChange.bind(this)}
            />
            <label
              className="mb-0"
              htmlFor="visibility"
            >
              Sichtbar
            </label>
          </div>

        </div>
      </div>
    );
  }
}

Size.propTypes = {
  visibility: PropTypes.string.isRequired,
  height: PropTypes.string.isRequired,
  width: PropTypes.string.isRequired,
  onStyleChange: PropTypes.func.isRequired,
};

export default Size;

import React, { Component } from 'react';
import PropTypes from 'prop-types';
import DlgColorPicker from '../../StdComponents/DlgColorPicker';

class Border extends Component {
  constructor(props) {
    super(props);
    this.state = {
    };
  }

  handleColorChange(e) {
    this.props.onStyleChange('border-color', e);
  }

  handleSizeChange(e) {
    this.props.onStyleChange('border-width', `${e.target.value}px`);
  }

  handleRadiusChange(e) {
    this.props.onStyleChange('border-radius', `${e.target.value}vh`);
  }

  render() {
    // dicke in Zahl umwandeln
    const d = this.props.dicke ? parseFloat(this.props.dicke) : 0;

    // radius in Zahl umwandeln
    const r = this.props.radius ? parseFloat(this.props.radius) : 0;

    return (
      <div className="border border-dark">

        <div className="text-white bg-secondary pl-1 pr-1">Rahmen</div>

        <div className="d-flex flex-row pt-1 pl-1 pr-1">

          {/* Color */}
          <DlgColorPicker
            toolTip="Rahmenfarbe"
            color={this.props.color}
            onChange={this.handleColorChange.bind(this)}
          />

        </div>

        <div className="d-flex flex-row pt-1 pl-1 pr-1">

          {/* dicke */}
          <div className="input-group-prepend m-0">
            <div className="input-group-text p-0 pl-1 pr-1" style={{ width: '20px' }}>D</div>
          </div>
          <input
            title="Dicke in Pixel"
            type="number"
            className=""
            style={{ width: '50px' }}
            min="0"
            max="100"
            value={d}
            onChange={this.handleSizeChange.bind(this)}
          />
        </div>

        <div className="d-flex flex-row pt-1 pl-1">

          {/* Radius */}
          <div className="input-group-prepend m-0">
            <div className="input-group-text p-0 pl-1 pr-1" style={{ width: '20px' }}>R</div>
          </div>
          <input
            title="Eckradius in % der Anzeigehöhe"
            type="number"
            className=""
            style={{ width: '50px' }}
            min="0"
            max="100"
            value={r}
            onChange={this.handleRadiusChange.bind(this)}
          />
        </div>

      </div>
    );
  }
}

Border.propTypes = {
  dicke: PropTypes.string.isRequired,
  radius: PropTypes.string.isRequired,
  color: PropTypes.string.isRequired,
  onStyleChange: PropTypes.func.isRequired,
};

export default Border;

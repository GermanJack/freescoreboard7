import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { BsTypeBold, BsTypeItalic } from 'react-icons/bs';
import DlgColorPicker from '../../StdComponents/DlgColorPicker';
import DropdownFont from '../../StdComponents/DropdownFont';

class Font extends Component {
  constructor(props) {
    super(props);
    this.state = {
      fontList: [
        'Arial',
        'Helvetica',
        'Times New Roman',
        'Courier', 'Verdana',
        'Georgia',
        'Palatino',
        'Garamond',
        'Bookman',
        'Comic Sans MS',
        'Trebuchet MS',
        'Arial Black',
        'Impact',
        'ScoreBoard',
      ],
    };
  }

  handleColorChange(color) {
    this.props.onStyleChange('color', color);
  }

  handleSizeChange(e) {
    this.props.onStyleChange('font-size', `${e.target.value}vh`);
  }

  handleFontSelection(e) {
    this.props.onStyleChange('font-family', e);
  }

  handleBoldClick() {
    const value = this.props.weight === 'bold' ? 'normal' : 'bold';
    this.props.onStyleChange('font-weight', value);
  }

  handleItalicClick() {
    const value = this.props.style === 'italic' ? 'normal' : 'italic';
    this.props.onStyleChange('font-style', value);
  }

  render() {
    // fontsize in Zahl umwandeln
    const s = this.props.size ? parseFloat(this.props.size) : 0;

    // class für aktivierte btn ausrechnen
    const activeClass = 'btn btn-outline-secondary btn-icon p-0 pl-1 pr-1 pb-1 btn-selected';
    const inactiveClass = 'btn btn-outline-secondary btn-icon p-0 pl-1 pr-1 pb-1';
    const cb = this.props.weight === 'bold' ? activeClass : inactiveClass;
    const ci = this.props.style === 'italic' ? activeClass : inactiveClass;

    const fl = this.props.fontList.map((f) => f.fontname);
    const complFontList = fl ? fl.concat(this.state.fontList) : this.state.fontList;
    complFontList.sort();

    return (
      <div className="border border-dark">

        <div className="text-white bg-secondary pl-1 pr-1">Schrift</div>

        <div className="d-flex flex-row pt-1 pl-1 pr-1">

          {/*  Font Family */}
          <DropdownFont
            className=""
            data-toggle="tooltip"
            data-placement="right"
            title="Schriftart"
            values={complFontList}
            value={this.props.font}
            selection={this.handleFontSelection.bind(this)}
          />
        </div>

        <div className="d-flex flex-row pt-1 pl-1 pr-1">

          {/* size */}
          <input
            title="Schriftgröße in % der Anzeigehöhe"
            type="number"
            className=""
            style={{ width: '50px' }}
            min="0"
            max="50"
            step="0.5"
            value={s}
            onChange={this.handleSizeChange.bind(this)}
          />

          {/* color */}
          <DlgColorPicker
            toolTip="Schriftfarbe"
            color={this.props.color}
            onChange={this.handleColorChange.bind(this)}
          />

          {/* bold */}
          <button
            type="button"
            className={cb}
            data-toggle="tooltip"
            data-placement="right"
            title="fett"
            onClick={this.handleBoldClick.bind(this)}
          >
            <BsTypeBold size="1.2em" />
          </button>

          {/* italic */}
          <button
            type="button"
            className={ci}
            data-toggle="tooltip"
            data-placement="right"
            title="schräg"
            onClick={this.handleItalicClick.bind(this)}
          >
            <BsTypeItalic size="1.2em" />
          </button>

        </div>

      </div>
    );
  }
}

Font.propTypes = {
  fontList: PropTypes.arrayOf(PropTypes.object).isRequired,
  weight: PropTypes.string.isRequired,
  style: PropTypes.string.isRequired,
  size: PropTypes.string.isRequired,
  font: PropTypes.string.isRequired,
  color: PropTypes.string.isRequired,
  onStyleChange: PropTypes.func.isRequired,
};

export default Font;

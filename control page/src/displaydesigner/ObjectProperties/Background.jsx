import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { AiOutlinePicture } from 'react-icons/ai';
import DlgColorPicker from '../../StdComponents/DlgColorPicker';
import Dropdown from '../../StdComponents/Dropdown';
import DropdownBildMod from '../../StdComponents/DropdownBildMod';
import DlgPicSelection from '../../StdComponents/DlgPicSelection';

class Background extends Component {
  constructor(props) {
    super(props);
    this.state = {
    };
  }

  handleColorChange(color) {
    this.props.onStyleChange('background-color', color);
  }

  handleBildModChange(e) {
    // this.props.onStyleChange('background-size', e);
    if (e === 'repeat') {
      this.props.onStyleChange('background-repeat', 'repeat');
      this.props.onStyleChange('background-position', 'initial');
      this.props.onStyleChange('background-size', 'auto');
    } else {
      this.props.onStyleChange('background-repeat', 'no-repeat');
      this.props.onStyleChange('background-position', 'center');
      this.props.onStyleChange('background-size', e);
    }
  }

  handlePicSelection(e) {
    // console.log(e);
    if (e === '') {
      this.props.onStyleChange('background-image', '');
    } else {
      this.props.onStyleChange('background-image', `url('../../pictures/${e}')`);
    }
  }

  handleVarSelection(e) {
    if (e !== '[kein dyn. Bild]') {
      const vl = this.props.picVariables;
      const vi = vl.findIndex((x) => x.Variable === e);
      const v = this.props.picVariables[vi];
      this.props.onAtributeChange('bgid', v.ID);
    } else {
      this.props.onAtributeChange('bgid', 'B00');
    }
  }

  render() {
    const vl = [];
    this.props.picVariables.sort((a, b) => ((a.Variable > b.Variable) ? 1 : ((b.Variable > a.Variable) ? -1 : 0)));
    for (let i = 0; i < this.props.picVariables.length; i += 1) {
      const v = this.props.picVariables[i];
      vl.push(v.Variable);
    }

    let cidBildind = this.props.picVariables.findIndex((x) => x.ID === this.props.bildvariable);
    if (cidBildind === -1) {
      cidBildind = 0;
    }

    const v = this.props.picVariables[cidBildind];
    const bgid = v ? v.Variable : 'B00';

    return (
      <div className="border border-dark">
        <div className="text-white bg-secondary pl-1 pr-1">Hintergrund</div>

        <div className="d-flex flex-row pt-1 pl-1 pr-1">

          {/*  BildVariable */}
          <Dropdown
            className=""
            toolTip="dynamisches Bild"
            data-toggle="tooltip"
            data-placement="right"
            title="dyn. Bild"
            values={vl}
            value={bgid}
            wahl={this.handleVarSelection.bind(this)}
          />
        </div>

        <div className="d-flex flex-row pt-1 pl-1">

          {/* color */}
          <DlgColorPicker
            toolTip="Hintergrundfarbe"
            color={this.props.color}
            onChange={this.handleColorChange.bind(this)}
          />

          {/* Bild */}
          <DlgPicSelection
            label1="Bildwahl:"
            text=""
            class="btn btn-outline-secondary btn-sm"
            reacticon={<AiOutlinePicture size="1.2em" />}
            data-toggle="tooltip"
            data-placement="right"
            toolTip="fixes Bild"
            modalID="Modal-H1"
            values={this.props.picList}
            selection={this.handlePicSelection.bind(this)}
          />

          {/* gestreckt */}
          <DropdownBildMod
            value={this.props.bildMod}
            onChange={this.handleBildModChange.bind(this)}
          />

        </div>
      </div>
    );
  }
}

Background.propTypes = {
  picVariables: PropTypes.arrayOf(PropTypes.object).isRequired,
  bildvariable: PropTypes.string.isRequired,
  picList: PropTypes.arrayOf(PropTypes.object).isRequired,
  bildMod: PropTypes.string.isRequired,
  color: PropTypes.string.isRequired,
  onStyleChange: PropTypes.func.isRequired,
  onAtributeChange: PropTypes.func.isRequired,
};

export default Background;

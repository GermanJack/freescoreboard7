import React, { Component } from 'react';
import PropTypes from 'prop-types';
import Dropdown from '../../StdComponents/Dropdown';
import DropdownHAlign from '../../StdComponents/DropdownHAlign';
import DropdownVAlign from '../../StdComponents/DropdownVAlign';

class Content extends Component {
  constructor(props) {
    super(props);
    this.txtRef = React.createRef();
    this.spdRef = React.createRef();
    this.state = {
    };
  }

  handleTextChange(e) {
    this.props.onAtributeChange('innerText', e.target.value);
  }

  handleClickHAusrichtung(e) {
    // this.props.onHAlignChanged(e);
    this.props.onStyleChange('justify-content', e);
  }

  handleClickVAusrichtung(e) {
    // this.props.onVAlignChanged(e);
    this.props.onStyleChange('align-items', e);
  }

  handleVarSelection(e) {
    const vl = this.props.textVariables;
    const vi = vl.findIndex((x) => x.Variable === e);
    const v = this.props.textVariables[vi];
    this.props.onAtributeChange('textid', v.ID);
  }

  handleSpeedChange(e) {
    const speed = e.target.value;

    if (speed > 0) {
      this.props.onAtributeChange('Speed', speed);
      this.props.onStyleChange('overflow', 'hidden');
    } else {
      this.props.onAtributeChange('Speed', 0);
    }
  }

  render() {
    const vl = [];
    this.props.textVariables.sort((a, b) => ((a.Variable > b.Variable) ? 1 : ((b.Variable > a.Variable) ? -1 : 0)));
    for (let i = 0; i < this.props.textVariables.length; i += 1) {
      const v = this.props.textVariables[i];
      vl.push(v.Variable);
    }

    let textidTextind = this.props.textVariables.findIndex((x) => x.ID === this.props.variable);
    if (textidTextind === -1) {
      textidTextind = 0;
    }

    const v = this.props.textVariables[textidTextind];
    const textidText = v ? v.Variable : '';

    const s = this.props.speed ? parseFloat(this.props.speed) : 0;

    return (
      <div className="border border-dark">
        <div className="text-white bg-secondary pl-1 pr-1">Innhalt</div>

        <div className="d-flex flex-row pt-1 pl-1 pr-1">

          {/* Text */}
          <input
            type="text"
            className="form-control p-0 pl-1 pr-1"
            data-toggle="tooltip"
            title="statischer Text"
            value={this.props.text}
            ref={this.txtRef}
            onChange={this.handleTextChange.bind(this)}
          />
        </div>

        <div className="d-flex flex-row pt-1 pl-1">

          {/*  Text Variable */}
          <Dropdown
            className=""
            toolTip="dynamischer Text"
            data-toggle="tooltip"
            data-placement="right"
            title="dyn. Text"
            values={vl}
            value={textidText}
            wahl={this.handleVarSelection.bind(this)}
          />

        </div>

        <div className="d-flex flex-row pt-1 pl-1">

          {/* Ausrichtung */}
          <DropdownHAlign
            halign={this.props.ha}
            onClick={this.handleClickHAusrichtung.bind(this)}
          />

          <DropdownVAlign
            valign={this.props.va}
            onClick={this.handleClickVAusrichtung.bind(this)}
          />

          {/* Geschwindigkeit */}
          <input
            title="Geschwindigkeit"
            type="number"
            className=""
            style={{ width: '50px' }}
            min="0"
            max="20"
            value={s}
            ref={this.spdRef}
            onChange={this.handleSpeedChange.bind(this)}
          />

        </div>

      </div>
    );
  }
}

Content.propTypes = {
  textVariables: PropTypes.arrayOf(PropTypes.object).isRequired,
  variable: PropTypes.string.isRequired,
  text: PropTypes.string.isRequired,
  speed: PropTypes.string.isRequired,
  ha: PropTypes.string.isRequired,
  va: PropTypes.string.isRequired,
  onStyleChange: PropTypes.func.isRequired,
  onAtributeChange: PropTypes.func.isRequired,
};

export default Content;

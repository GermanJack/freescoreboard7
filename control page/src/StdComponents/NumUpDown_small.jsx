import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { GoPlus, GoDash } from 'react-icons/go';
import DlgTable from './DlgTable';

class NumUpDownsmall extends Component {
  handleClick(e) {
    this.props.click({ funcID: e.funcID, variable: this.props.variable, playerID: e.wahl });
  }

  render() {
    const ml = [];
    const plc = this.props.playerlist;
    if (plc) {
      for (let i = 0; i < plc.length; i += 1) {
        const p = plc[i];
        let sid = '';
        if (p.SID) {
          sid = ` (${p.SID})`;
        }

        ml.push({ ID: p.ID, Name: `${p.Vorname} ${p.Nachname} ${sid}` });
      }
    }

    const plusStyle = 'btn btn-success btn-sm border-dark btn-font-2 p-1';

    let plus = (
      <div>
        <button
          type="button"
          className={plusStyle}
          onClick={this.handleClick.bind(this, { playerID: '', funcID: '+' })}
        >
          <GoPlus size="1.5em" color="white" />
        </button>
      </div>
    );

    if (this.props.askPlayer === 'True' && this.props.playerlist.length > 0) {
      plus = (
        <DlgTable
          label1="Spieler wahl ..."
          text=""
          className={plusStyle}
          data-toggle="tooltip"
          data-placement="right"
          toolTip="Spieler wählen"
          icon=""
          reacticon={<GoPlus size="1.5em" color="white" />}
          iconAltText="+"
          modalID={`Modal-MFoul${this.props.variable}`}
          data={ml}
          funcID="+"
          wahl={this.handleClick.bind(this)}
        />
      );
    }

    return (
      <div className="d-flex justify-content-around">
        <div className="d-flex flex-row mr-2 ">
          <button
            type="button"
            className="btn btn-warning btn-sm border-dark btn-font-2 p-1"
            onClick={this.handleClick.bind(this, { playerID: '', funcID: '-' })}
          >
            <GoDash size="1.5em" color="black" />
          </button>
          <span className="btn btn-sm border-dark bg-light" data-labelid={this.props.Name}>{this.props.count}</span>
          {plus}
        </div>
      </div>
    );
  }
}

NumUpDownsmall.propTypes = {
  variable: PropTypes.string,
  playerlist: PropTypes.arrayOf(PropTypes.object),
  askPlayer: PropTypes.string,
  Name: PropTypes.string,
  count: PropTypes.number,
  click: PropTypes.func,
};

NumUpDownsmall.defaultProps = {
  variable: '',
  playerlist: [],
  askPlayer: 'False',
  Name: '',
  count: 0,
  click: () => {},
};

export default NumUpDownsmall;

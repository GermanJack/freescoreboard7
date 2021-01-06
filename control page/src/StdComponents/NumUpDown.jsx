import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { GoPlus, GoDash } from 'react-icons/go';
import DlgTable from './DlgTable';

class NumUpDown extends Component {
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

        if (p.Vorname !== null) {
          ml.push({ ID: p.ID, Name: `${p.Vorname} ${p.Nachname} ${sid}` });
        } else {
          ml.push({ ID: p.ID, Name: `${p.Nachname} ${sid}` });
        }
      }
    }

    const plusStyle = 'btn btn-success btn-sm border-dark btn-font-2 p-2 pt-1';

    let plus = (
      <div>
        <button type="button" className={plusStyle} onClick={this.handleClick.bind(this, { playerID: '', funcID: '+' })}>
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
          reacticon={<GoPlus size="1.5em" color="white" />}
          icon=""
          iconAltText="+"
          modalID={`Modal-MTor${this.props.variable}`}
          data={ml}
          funcID="+"
          wahl={this.handleClick.bind(this)}
        />
      );
    }

    return (
      <div className="mt-1">
        <div className="d-flex justify-content-around">
          <div className="d-flex flex-row">
            <div className="display-4 border border-dark pl-4 pr-4" data-dataid={this.props.dataid}>
              {this.props.count}
            </div>
            <div className="d-flex flex-column justify-content-between">
              {plus}

              <button
                type="button"
                className="btn btn-warning btn-sm border-dark"
                onClick={this.handleClick.bind(this, { playerID: '', funcID: '-' })}
              >
                {/* <IoIosRemove /> */}
                <GoDash size="1.5em" color="black" />
              </button>
            </div>
          </div>
        </div>
      </div>
    );
  }
}

NumUpDown.propTypes = {
  variable: PropTypes.string,
  playerlist: PropTypes.arrayOf(PropTypes.object),
  askPlayer: PropTypes.string,
  dataid: PropTypes.string,
  count: PropTypes.number,
  click: PropTypes.func,
};

NumUpDown.defaultProps = {
  variable: '',
  playerlist: [],
  askPlayer: 'False',
  dataid: '',
  count: 0,
  click: () => {},
};

export default NumUpDown;

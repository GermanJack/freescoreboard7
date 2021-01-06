/* eslint-disable jsx-a11y/control-has-associated-label */
/* eslint-disable jsx-a11y/label-has-associated-control */
import React, { Component } from 'react';
import PropTypes from 'prop-types';
import DropdownWithID from '../../StdComponents/DropdownWithID';
import Dropdown from '../../StdComponents/Dropdown';
import MinuteTime from '../../StdComponents/MinuteTime';

import TEreignisse from '../dataClasses/TEreignisse';
import ClsTurnier from '../dataClasses/ClsTurnier';

class DlgEvent extends Component {
  constructor() {
    super();
    this.state = {
    };
  }

  render() {
    // Spiel
    const sl = [];
    if (this.props.turnier.length > 0) {
      const sl1 = this.props.turnier[0].Spiele;
      sl1.map((x) => sl.push(x.Spiel));
    } else {
      sl.push(0);
    }

    // Ereignistypen
    const el = [];
    for (let i = 0; i < this.props.events.length; i += 1) {
      if (this.props.events.ID === 5 || this.props.events.ID === 7 || this.props.events.ID === 7) {
        const NID = `0${this.props.events.ID}`;
        el.push(
          { ID: NID, Text: this.props.events.Nummer },
        );
      }
    }

    // Teams
    const tl = [];
    const pl = [];
    if (this.props.turnier.length > 0) {
      const tl1 = this.props.turnier[0].Tabellen.filter((x) => x.Runde === 1);
      tl1.map((x) => tl.push(x.Mannschaft));
    } else {
      this.props.teamList.map((x) => tl.push(x.Name));
    }

    if (this.props.Event.Mannschaft !== null) {
      // Spieler
      const t = this.props.teamList.find((x) => x.Name === this.props.Event.Mannschaft);
      if (t !== undefined) {
        const pl1 = this.props.playerListAll;
        for (let i = 0; i < pl1.length; i += 1) {
          if (pl1[i].MannschaftsID === t.ID) {
            pl.push(`${pl1[i].Nachname}, ${pl1[i].Vorname}`);
          }
        }
      }
    }

    // Details
    let ed = null;
    if (this.props.Event.Ereignistyp === '07') {
      ed = (
        <div className="form-group row">
          <label htmlFor="mananz" className="col-sm-3 col-form-label">Details</label>
          <div className="col-sm-6">
            <input
              type="text"
              value={this.props.Event.Details}
              className="form-control form-control-sm mt-1 text-left"
              onChange={this.props.setWert.bind(this, 'Details')}
            />
          </div>
        </div>
      );
    }

    return (
      <div className="d-flex flex-column">
        <div className="">
          <div className="form-group row mt-1">
            <label htmlFor="spiel" className="col-sm-3 col-form-label">SpielNr.</label>
            <div className="col-sm-3">
              <Dropdown
                id="spiel"
                values={sl}
                value={this.props.Event.Spiel}
                cName="form-control form-control-sm mt-1 text-left"
                wahl={this.props.setWert.bind(this, 'Spiel')}
              />
            </div>
          </div>
          <div className="form-group row">
            <label htmlFor="mananz" className="col-sm-3 col-form-label">Ereignisart</label>
            <div className="col-sm-6">
              <DropdownWithID
                values={el}
                value={this.props.Event.Ereignistyp}
                cName="form-control form-control-sm mt-1 text-left"
                wahl={this.props.setWert.bind(this, 'Ereignisart')}
              />
            </div>
          </div>
          {ed}
          <div className="form-group row">
            <label htmlFor="platz" className="col-sm-3 col-form-label">Mannschaft</label>
            <div className="col-sm-6">
              <Dropdown
                id="platz"
                values={tl}
                value={this.props.Event.Mannschaft}
                cName="form-control form-control-sm mt-1 text-left"
                wahl={this.props.setWert.bind(this, 'Mannschaft')}
              />
            </div>
          </div>
          <div className="form-group row">
            <label htmlFor="besch" className="col-sm-3 col-form-label">Spieler</label>
            <div className="col-sm-6">
              <Dropdown
                values={pl}
                value={this.props.Event.Spieler}
                cName="form-control form-control-sm mt-1 text-left"
                wahl={this.props.setWert.bind(this, 'Spieler')}
              />
            </div>
          </div>
          <div className="form-group row">
            <label htmlFor="besch" className="col-sm-3 col-form-label">Spielzeit</label>
            <div className="col-sm-6">
              <MinuteTime
                seconds={this.props.Event.Spielzeit}
                TimeSet={this.props.setWert.bind(this, 'Spielzeit')}
              />
            </div>
          </div>
          <div className="form-group row">
            <label htmlFor="besch" className="col-sm-3 col-form-label">Spielabschnitt</label>
            <div className="col-sm-3">
              <input
                className="form-control"
                type="number"
                min="1"
                max="99"
                id="Abschnitt"
                value={this.props.Event.Spielabschnitt}
                spellCheck="false"
                onChange={this.props.setWert.bind(this, 'Abschnitt')}
                title="Spielabschnitt in dem das ereignis stattgefunden hat."
              />
            </div>
          </div>
        </div>
      </div>
    );
  }
}

DlgEvent.propTypes = {
  setWert: PropTypes.func.isRequired,
  Event: PropTypes.instanceOf(TEreignisse).isRequired,
  teamList: PropTypes.arrayOf(PropTypes.object).isRequired,
  playerListAll: PropTypes.arrayOf(PropTypes.object).isRequired,
  events: PropTypes.arrayOf(PropTypes.object).isRequired,
  turnier: PropTypes.instanceOf(ClsTurnier).isRequired,
};

DlgEvent.defaultProps = {
};

export default DlgEvent;

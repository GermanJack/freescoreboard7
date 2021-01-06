/* eslint-disable jsx-a11y/control-has-associated-label */
/* eslint-disable jsx-a11y/label-has-associated-control */
import React, { Component } from 'react';
import PropTypes from 'prop-types';
import DropdownWithID from '../../StdComponents/DropdownWithID';

import TKopf from '../dataClasses/TKopf';

class System1 extends Component {
  constructor() {
    super();
    this.state = {
      Kopf: new TKopf(),
    };
  }

  componentDidMount() {
    const k = new TKopf();
    k.Mananz = 2;
    k.Platzierungstyp = 'P';
    k.Turniertyp = 'System';
    k.Beschreibung = '';
    k.Kommentar = '';
    this.setState({ Kopf: k });
  }

  handleNameChange(e) {
    // eslint-disable-next-line react/no-access-state-in-setstate
    const k = this.state.Kopf;
    k.Beschreibung = e.target.value;
    this.setState({ Kopf: k });
  }

  handleAnzChange(e) {
    // eslint-disable-next-line react/no-access-state-in-setstate
    const k = this.state.Kopf;
    k.Mananz = parseInt(e.target.value, 10);
    this.setState({ Kopf: k });
  }

  handlePlatzChange(e) {
    // eslint-disable-next-line react/no-access-state-in-setstate
    const k = this.state.Kopf;
    k.Platzierungstyp = e;
    this.setState({ Kopf: k });
  }

  handleBeschChange(e) {
    // eslint-disable-next-line react/no-access-state-in-setstate
    const k = this.state.Kopf;
    k.Kommentar = e.target.value;
    this.setState({ Kopf: k });
  }

  weiter() {
    if (this.props.runden.length > 0) {
      this.props.setDaten('rundenLeer');
    }

    this.props.setDaten('Kopf', this.state.Kopf);

    this.props.clickWeiter(1);
  }

  render() {
    const head = (
      <div>
        <b>Grunddaten</b>
        {' > Runden > Gruppen > Spiele'}
      </div>
    );

    let weiter = true;
    let txt = '';
    if (this.state.Kopf.Beschreibung === '') {
      txt = 'Name fehlt';
    }

    if (this.props.namensliste.includes(this.state.Kopf.Beschreibung)) {
      txt = 'Name bereits vorhanden';
    }

    if (txt !== '') {
      weiter = false;
    }

    return (
      <div className="d-flex flex-column">
        {head}

        <div className="">
          <div className="form-group row mt-1">
            <label htmlFor="name" className="col-sm-3 col-form-label">Name</label>
            <div className="col-sm-5">
              <input
                type="Text"
                className="form-control form-control-sm mt-1"
                id="name"
                value={this.state.Kopf.Beschreibung}
                spellCheck="false"
                onChange={this.handleNameChange.bind(this)}
                title="Name des Turniesystems"
              />
            </div>
          </div>
          <div className="form-group row">
            <label htmlFor="mananz" className="col-sm-3 col-form-label">anz. Mannsch.</label>
            <div className="col-sm-2">
              <input
                type="number"
                className="form-control form-control-sm mt-1"
                min="2"
                max="99"
                id="mananz"
                value={this.state.Kopf.Mananz}
                spellCheck="false"
                onChange={this.handleAnzChange.bind(this)}
                title="Anzahl der Mannschaften für das Turniesystems"
              />
            </div>
          </div>
          <div className="form-group row">
            <label htmlFor="platz" className="col-sm-3 col-form-label">Platzierung</label>
            <div className="col-sm-5">
              <DropdownWithID
                id="platz"
                values={
                  [
                    { ID: 'P', Text: 'über Platzangabe an Spielen (P)' },
                    { ID: 'T', Text: 'über Gruppentabelle der Finalgruppe (T)' },
                    { ID: 'O', Text: 'ohne Platzierung (O)' },
                  ]
                }
                value={this.state.Kopf.Platzierungstyp}
                cName="form-control form-control-sm mt-1 text-left"
                wahl={this.handlePlatzChange.bind(this)}
              />
            </div>
          </div>
          <div className="form-group row">
            <label htmlFor="besch" className="col-sm-3 col-form-label">Beschreibung</label>
            <div className="col-sm-5">
              <textarea
                className="form-control form-control-sm mt-1"
                id="besch"
                rows="5"
                value={this.state.Kopf.Kommentar}
                spellCheck="true"
                onChange={this.handleBeschChange.bind(this)}
              />
            </div>
          </div>
        </div>

        <div className="d-flex flkex-row justify-content-end mt-1">
          <button
            type="button"
            className="btn btn-primary border-dark m-1"
            onClick={this.weiter.bind(this)}
            disabled={!weiter}
          >
            Weiter
          </button>
        </div>
        <div className="text-danger mt-1 mr-1">{txt}</div>
      </div>
    );
  }
}

System1.propTypes = {
  namensliste: PropTypes.arrayOf(PropTypes.string),
  runden: PropTypes.arrayOf(PropTypes.object),
  clickWeiter: PropTypes.func,
  setDaten: PropTypes.func,
};

System1.defaultProps = {
  namensliste: [],
  runden: [],
  clickWeiter: () => {},
  setDaten: () => {},
};

export default System1;

/* eslint-disable jsx-a11y/label-has-associated-control */
import React, { Component } from 'react';
import PropTypes from 'prop-types';
import DropdownWithID from '../../StdComponents/DropdownWithID';

import TKopf from '../dataClasses/TKopf';
import ClsTurnier from '../dataClasses/ClsTurnier';

class Turnier1 extends Component {
  constructor() {
    super();
    this.state = {
      Kopf: new TKopf(),
      systemNamensListe: [],
    };
  }

  componentDidMount() {
    const k = new TKopf();
    k.Turniertyp = 'Turnier';
    k.Beschreibung = '';
    k.Matrix = '';
    k.Kommentar = '';
    this.setState({ Kopf: k });
  }

  componentDidUpdate(prevProps) {
    if (prevProps.systeme !== this.props.systeme) {
      // eslint-disable-next-line react/no-did-update-set-state
      this.setState({ systemNamensListe: this.systemNamensListe() });
    }
  }

  systemNamensListe() {
    const data = [];

    data.push({
      ID: 0,
      Text: 'Ad Hoc Turnier',
    });

    for (let i = 0; i < this.props.systeme.length; i += 1) {
      data.push({
        ID: this.props.systeme[i].Kopf.ID,
        Text: this.props.systeme[i].Kopf.Beschreibung,
      });
    }

    return data;
  }

  systemManAnz(e) {
    let data = 0;
    if (e > 0) {
      const s = this.props.systeme.filter((x) => x.Kopf.ID === e);
      data = s[0].Kopf.Mananz;
    }
    return data;
  }

  handleNameChange(e) {
    // eslint-disable-next-line react/no-access-state-in-setstate
    const k = this.state.Kopf;
    k.Beschreibung = e.target.value;
    this.setState({ Kopf: k });
  }

  handleLigaChange(e) {
    // eslint-disable-next-line react/no-access-state-in-setstate
    const k = this.state.Kopf;
    k.Liga = e.target.value;
    this.setState({ Kopf: k });
  }

  handleBeschChange(e) {
    // eslint-disable-next-line react/no-access-state-in-setstate
    const k = this.state.Kopf;
    k.Kommentar = e.target.value;
    this.setState({ Kopf: k });
  }

  handleSystemChange(e) {
    // eslint-disable-next-line react/no-access-state-in-setstate
    const k = this.state.Kopf;
    k.MatrixID = e;
    if (e === 0) {
      k.Matrix = 'Ad Hoc Turnier';
    } else {
      const m = this.props.systeme.filter((x) => x.Kopf.ID === e);
      k.Matrix = m[0].Kopf.Beschreibung;
    }
    this.setState({ Kopf: k });
  }

  weiter() {
    const t = this.props.turnier;
    t.Kopf = this.state.Kopf;
    this.props.setDaten('turnier', t);

    this.props.clickWeiter(1);
  }

  render() {
    const head = (
      <div>
        <b>Grunddaten</b>
        {' > Mannschaften > Spiele'}
      </div>
    );

    let weiter = true;
    let txt = '';
    if (this.systemManAnz(this.state.Kopf.MatrixID) > this.props.teamList.length) {
      txt = 'Zu wenige Mannschaften im Mannschaften-Stamm für diese Turniersystem';
    }

    if (this.state.Kopf.Matrix === '') {
      txt = 'Turniersystem fehlt';
    }

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
            <label htmlFor="platz" className="col-sm-3 col-form-label">Turniersystem</label>
            <div className="col-sm-5">
              <DropdownWithID
                id="Turniersystem"
                values={this.state.systemNamensListe}
                value={this.state.Kopf.MatrixID}
                cName="form-control form-control-sm mt-1 text-left"
                wahl={this.handleSystemChange.bind(this)}
              />
            </div>
          </div>
          <div className="form-group row mt-1">
            <label htmlFor="name" className="col-sm-3 col-form-label">Liga</label>
            <div className="col-sm-5">
              <input
                type="Text"
                className="form-control form-control-sm mt-1"
                id="Liga"
                value={this.state.Kopf.Liga}
                spellCheck="false"
                onChange={this.handleLigaChange.bind(this)}
                title="Liga"
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

Turnier1.propTypes = {
  namensliste: PropTypes.arrayOf(PropTypes.string),
  systeme: PropTypes.arrayOf(PropTypes.object),
  teamList: PropTypes.arrayOf(PropTypes.object),
  clickWeiter: PropTypes.func,
  setDaten: PropTypes.func,
  turnier: PropTypes.instanceOf(ClsTurnier),
};

Turnier1.defaultProps = {
  namensliste: [],
  systeme: [],
  teamList: [],
  clickWeiter: () => {},
  setDaten: () => {},
  turnier: new ClsTurnier(),
};

export default Turnier1;

/* eslint-disable react/no-did-update-set-state */
/* eslint-disable jsx-a11y/label-has-associated-control */
import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { CgPlayListAdd, CgPlayListRemove } from 'react-icons/cg';
import Table from '../../StdComponents/Table';
import TKopf from '../dataClasses/TKopf';
import TRunden from '../dataClasses/TRunden';

class System2 extends Component {
  constructor() {
    super();
    this.state = {
      runde: 1,
      mananz: 2,
      gruppenanz: 1,
      rundenname: '',
    };
  }

  componentDidUpdate() {
    if (this.state.runde === 1) {
      if (this.state.mananz * this.state.gruppenanz !== this.props.SystemKopf.Mananz) {
        // eslint-disable-next-line react/no-access-state-in-setstate
        this.setState({ mananz: this.props.SystemKopf.Mananz / this.state.gruppenanz });
      }
    }

    if (this.props.runden.length + 1 !== this.state.runde) {
      this.setState({ runde: this.props.runden.length + 1 });
    }
  }

  handleMAnzChange(e) {
    this.setState({ mananz: e.target.value });
  }

  handleGAnzChange(e) {
    this.setState({ gruppenanz: e.target.value });
  }

  handleNameChange(e) {
    this.setState({ rundenname: e.target.value });
  }

  rundeRein() {
    const r = new TRunden();
    r.Runde = this.state.runde;
    r.AnzGrp = this.state.gruppenanz;
    r.AnzMann = this.state.mananz;
    r.Rundenname = this.state.rundenname;

    this.props.setDaten('runde', r);
    this.setState((prevState) => ({ runde: prevState.runde + 1 }));
  }

  rundeRaus() {
    this.props.setDaten('rundeRaus');
    this.setState((prevState) => ({ runde: prevState.runde - 1 }));
  }

  render() {
    const head = (
      <div>
        {'Grunddaten > '}
        <b>Runden</b>
        {' > Gruppen > Spiele'}
      </div>
    );

    let disableRein = false;
    let message = '';
    if (this.state.mananz * this.state.gruppenanz > this.props.SystemKopf.Mananz) {
      message = 'Zu viele Mannschaften eingeplant!';
      disableRein = true;
    }
    if (this.state.runde === 1
      && this.state.mananz * this.state.gruppenanz < this.props.SystemKopf.Mananz) {
      message = 'Zu wenige Mannschaften eingeplant!';
      disableRein = true;
    }
    if (this.state.runde === 1 && this.state.mananz % 1 !== 0) {
      message = 'Falsche Anzahl Mannschaften!';
      disableRein = true;
    }

    let disableWeiter = false;
    let disableRaus = false;
    let wmessage = '';

    if (this.props.SystemKopf.Platzierungstyp === 'T' && this.state.gruppenanz > 1) {
      wmessage = 'Bei Platzierungstyp T darf in der letzten Runde nur eine Gruppe sein.';
      disableWeiter = true;
    }

    if (this.props.runden.length === 0) {
      disableWeiter = true;
      disableRaus = true;
      wmessage = 'Noch keine Runde definiert.';
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
                value={this.props.SystemKopf.Beschreibung}
                spellCheck="false"
                readOnly
              />
            </div>
          </div>

          <div className="d-flex flex-row">
            <div className="mr-2">
              <label htmlFor="runde" className="col-form-label">Runden-Nr.</label>
              <div className="">
                <input
                  type="number"
                  className="form-control form-control-sm"
                  min="1"
                  max="99"
                  id="runde"
                  value={this.state.runde}
                  readOnly
                />
              </div>
            </div>
            <div className="mr-2">
              <label htmlFor="grp" className="col-form-label">anz. Gruppen</label>
              <div className="">
                <input
                  type="number"
                  className="form-control form-control-sm"
                  min="1"
                  max="99"
                  id="grp"
                  value={this.state.gruppenanz}
                  onChange={this.handleGAnzChange.bind(this)}
                />
              </div>
            </div>
            <div className="mr-2">
              <label htmlFor="mananz" className="col-form-label">anz. Mannsch.</label>
              <div className="">
                <input
                  type="number"
                  className="form-control form-control-sm"
                  title="Anzahl der Mannschaften je Gruppe"
                  min="2"
                  max={this.props.SystemKopf.Platzierungstyp}
                  id="mananz"
                  value={this.state.mananz}
                  onChange={this.handleMAnzChange.bind(this)}
                />
              </div>
            </div>
            <div className="mr-2">
              <label htmlFor="nam" className="col-form-label">Rundenname</label>
              <div className="">
                <input
                  type="text"
                  className="form-control form-control-sm"
                  id="nam"
                  value={this.state.rundenname}
                  onChange={this.handleNameChange.bind(this)}
                  title="Rundenname z.B.: Halbfinale"
                />
              </div>
            </div>
          </div>
        </div>

        <div className="d-flex flex-row">
          <button
            type="button"
            className="btn btn-primary border-dark m-1"
            onClick={this.rundeRein.bind(this)}
            disabled={disableRein}
            title="Runde in Planung einfügen"
          >
            <CgPlayListAdd size="1.2em" />
          </button>

          <button
            type="button"
            className="btn btn-danger border-dark m-1"
            onClick={this.rundeRaus.bind(this)}
            disabled={disableRaus}
            title="Letzte Runde aus Planung entfernen"
          >
            <CgPlayListRemove size="1.2em" />
          </button>

          <div className="mr-1 pt-1">{message}</div>
        </div>

        <div className="d-flex flex-row">
          <Table
            daten={this.props.runden}
            cols={[
              { Column: 'Runde', Label: 'Runde' },
              { Column: 'AnzGrp', Label: 'anz. Gruppen' },
              { Column: 'AnzMann', Label: 'anz. Mannschaften' },
              { Column: 'Rundenname', Label: 'Rundenname' },
            ]}
            chkid={[]}
            chk=""
            chktype="radio"
            radiogrp="runden"
            height="20vh"
          />
        </div>

        <div className="d-flex flex-row justify-content-between">
          <button type="button" className="btn btn-primary border-dark m-1" onClick={this.props.clickWeiter.bind(this, -1)}>
            Zurück
          </button>
          <button type="button" className="btn btn-primary border-dark m-1" onClick={this.props.clickWeiter.bind(this, 1)} disabled={disableWeiter}>
            Weiter
          </button>
        </div>
        <div className="text-danger mr-1 pt-1">{wmessage}</div>
      </div>
    );
  }
}

System2.propTypes = {
  SystemKopf: PropTypes.objectOf(TKopf),
  runden: PropTypes.arrayOf(PropTypes.object),
  clickWeiter: PropTypes.func,
  setDaten: PropTypes.func,
};

System2.defaultProps = {
  SystemKopf: '',
  runden: [],
  clickWeiter: () => {},
  setDaten: () => {},
};

export default System2;

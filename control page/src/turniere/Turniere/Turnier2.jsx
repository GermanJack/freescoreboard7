/* eslint-disable react/no-access-state-in-setstate */
/* eslint-disable jsx-a11y/label-has-associated-control */
import React, { Component } from 'react';
import PropTypes from 'prop-types';
import Table from '../../StdComponents/Table';

import ClsTurnier from '../dataClasses/ClsTurnier';
import TTabellen from '../dataClasses/TTabellen';

class Turnier2 extends Component {
  constructor() {
    super();
    this.state = {
      manID: [],
      grpID: [],
      panel: 0,
    };
  }

  // eslint-disable-next-line no-unused-vars
  componentDidUpdate(prevProps, prevState) {
    if (this.props.panel !== this.state.panel) {
      // eslint-disable-next-line react/no-did-update-set-state
      this.setState({ panel: this.props.panel });
    }
  }

  rowClickMan(e) {
    const hlp = this.state.manID;
    if (this.state.manID.includes(e)) {
      const ind = hlp.findIndex((x) => x === e);
      hlp.splice(ind, 1);
    } else {
      hlp.push(e);
    }
    this.setState({ manID: hlp });
  }

  rowClickGrp(e) {
    const hlp = this.state.grpID;
    if (this.state.grpID.includes(e)) {
      const ind = hlp.findIndex((x) => x === e);
      hlp.splice(ind, 1);
    } else {
      hlp.push(e);
    }
    this.setState({ grpID: hlp });
  }

  Rein() {
    for (let i = 0; i < this.state.manID.length; i += 1) {
      const m = this.props.teamList.find((x) => x.ID === this.state.manID[i]);
      const mannschaft = m.Name;

      if (this.props.turnier.Kopf.MatrixID === 0) {
        // AddHock
        const gruppe = '1A';
        this.MannschaftInTabellenHinzufügen(mannschaft, gruppe, '01', null);
      } else {
        this.MannschaftInTabellenEintragen(mannschaft);
      }
    }
    this.setState({ manID: [] });
  }

  MannschaftInTabellenEintragen(Mannschaft) {
    const t = this.props.turnier.Tabellen;
    const tr = t.filter((x) => x.Runde === 1 && x.Mannschaft === '');
    const frei = tr[0];
    frei.Mannschaft = Mannschaft;
  }

  MannschaftInTabellenHinzufügen(Mannschaft, Gruppe, Quelltyp, Quellgruppe) {
    const t = new TTabellen();
    t.ID = this.props.turnier.Tabellen.length + 1;
    t.TurnierID = this.props.turnier.Kopf.ID;
    t.Runde = 1;
    t.Gruppe = Gruppe;
    t.Mannschaft = Mannschaft;
    t.Quelltyp = Quelltyp;
    t.QuellGruppe = Quellgruppe;

    this.props.turnier.Tabellen.push(t);
  }

  Raus() {
    if (this.state.grpID.length === 0) {
      return;
    }

    for (let i = 0; i < this.state.grpID.length; i += 1) {
      const t = this.props.turnier.Tabellen;
      const tr = t.filter((x) => x.ID === this.state.grpID[i]);
      if (this.props.turnier.Kopf.MatrixID === 0) {
        // AdHoc
        const removeIndex = this.props.turnier.Tabellen.map(
          (x) => x.ID,
        ).indexOf(this.state.grpID[i]);
        this.props.turnier.Tabellen.splice(removeIndex, 1);
      } else {
        tr[0].Mannschaft = '';
      }
    }
    this.setState({ grpID: [] });
  }

  selAllManClick(e) {
    if (!e.target.checked) {
      this.setState({ manID: [] });
    }

    if (e.target.checked) {
      this.setState({ manID: this.restTeams().map((x) => x.ID) });
    }
  }

  selAllGrpClick(e) {
    if (!e.target.checked) {
      this.setState({ grpID: [] });
    }

    if (e.target.checked) {
      this.setState({ grpID: this.mannEingrupiert().map((x) => x.ID) });
    }
  }

  mannEingrupiert() {
    const t = this.props.turnier.Tabellen;
    return t.filter((x) => x.Runde === 1);
  }

  restTeams() {
    const mannSchonDrin = this.mannEingrupiert().map((x) => x.Mannschaft);
    const restTeams = [];
    for (let i = 0; i < this.props.teamList.length; i += 1) {
      if (!mannSchonDrin.includes(this.props.teamList[i].Name)) {
        restTeams.push(this.props.teamList[i]);
      }
    }
    return restTeams;
  }

  render() {
    const head = (
      <div>
        {'Grunddaten > '}
        <b>Mannschaften</b>
        {' > Spiele'}
      </div>
    );

    const sMID = this.state.manID ? this.state.manID : '';
    const sGID = this.state.grpID ? this.state.grpID : '';

    let message = '';
    let disableWeiter = false;

    const tr = this.mannEingrupiert();

    const restTeams = this.restTeams();

    const freiGruppe = tr.filter((x) => x.Mannschaft === '');
    if (freiGruppe.length > 0) {
      message = 'Nicht alle Gruppen komplett!';
      disableWeiter = true;
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
                value={this.props.turnier.Kopf.Beschreibung}
                spellCheck="false"
                readOnly
              />
            </div>
          </div>
        </div>

        <div className="d-flex flex-row m-2">

          <Table
            id="Man"
            daten={restTeams}
            cols={[{ Column: 'Name', Label: 'Mannschaft' }]}
            chk="Selected"
            chktype="checkbox"
            chkid={sMID}
            radiogrp="Mannschaft"
            height="20vh"
            rowClick={this.rowClickMan.bind(this)}
            selAllClick={this.selAllManClick.bind(this)}
          />

          <div className="d-flex flex-column mt-4">
            <button
              type="button"
              className="btn btn-primary border-dark m-1"
              onClick={this.Rein.bind(this)}
              title="Mannschaft in Gruppe einfügen"
            >
              {/* <img
                src={require('../Icons/icons8-insert-row-16-withe.png')}
                alt="Rein"
              /> */}
              &gt;&gt;
            </button>
            <button
              type="button"
              className="btn btn-danger border-dark m-1"
              onClick={this.Raus.bind(this)}
              title="Mannschaft aus Gruppe entfernen"
            >
              {/* <img
                src={require('../Icons/icons8-delete-row-16-withe.png')}
                alt="Rein"
              /> */}
              &lt;&lt;
            </button>
          </div>

          <Table
            id="GRP"
            daten={tr}
            cols={[{ Column: 'Gruppe', Label: 'Gruppe' }, { Column: 'Mannschaft', Label: 'Mannschaft' }]}
            chk="Selected"
            chktype="checkbox"
            chkid={sGID}
            radiogrp="Gruppe"
            height="20vh"
            rowClick={this.rowClickGrp.bind(this)}
            selAllClick={this.selAllGrpClick.bind(this)}
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
        <div className="text-danger mr-1 pt-1">{message}</div>
      </div>
    );
  }
}

Turnier2.propTypes = {
  turnier: PropTypes.instanceOf(ClsTurnier),
  teamList: PropTypes.arrayOf(PropTypes.object),
  clickWeiter: PropTypes.func,
  panel: PropTypes.string,
};

Turnier2.defaultProps = {
  turnier: new ClsTurnier(),
  teamList: [],
  clickWeiter: () => {},
  panel: 0,
};

export default Turnier2;

import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { Graphviz } from 'graphviz-react';
import GraphCalc from '../GraphCalc';
import ClsTurnier from '../dataClasses/ClsTurnier';
import Table from '../../StdComponents/Table';

class SystemDetails extends Component {
  constructor(props) {
    super(props);
    this.state = {
    };
  }

  setTurnierText() {
    if (this.props.system === null) {
      return [];
    }
    const sys = this.props.system[0];
    const text = [];
    text.push(`${sys.Kopf.Turniertyp}Nr.: ${sys.Kopf.TurnierNr} (${sys.Kopf.ID})`);
    text.push(`Name: ${sys.Kopf.Beschreibung}`);
    text.push(`Mannschaften: ${sys.Kopf.Mananz}`);
    text.push(`Runden: ${sys.Runden.length}`);
    text.push(`Spiele: ${sys.Spiele.length}`);
    text.push('--------------------');
    text.push(`${sys.Kopf.Kommentar}`);
    return text;
  }

  render() {
    let t = null;
    let s = [];

    // Anzeigeblöcke
    let g = null;
    let tInfo = '';
    let spielplan = null;

    if (this.props.system !== null) {
      // eslint-disable-next-line prefer-destructuring
      t = this.props.system[0];
      s = t.Spiele;

      // Turnierbeschreibung
      tInfo = (
        <div className="p-1 mt-1 border border-dark">
          <div>
            Turnierinfos:
          </div>
          <div className="w-50">
            <table className="table table-sm table-borderless">
              <tr className="">
                <td>{`${t.Kopf.Turniertyp}Nr.: `}</td>
                <td>{`${t.Kopf.TurnierNr} (DBID: ${t.Kopf.ID})`}</td>
              </tr>
              <tr className="">
                <td>{'Name: '}</td>
                <td>{t.Kopf.Beschreibung}</td>
              </tr>
              <tr className="">
                <td>{'Liga: '}</td>
                <td>{t.Kopf.Liga}</td>
              </tr>
              <tr className="">
                <td>{'Mannschaften: '}</td>
                <td>{t.Kopf.Mananz}</td>
              </tr>
              <tr className="">
                <td>{'Runden: '}</td>
                <td>{t.Runden.length}</td>
              </tr>
              <tr className="">
                <td>{'Spiele: '}</td>
                <td>{t.Spiele.length}</td>
              </tr>
              <tr className="">
                <td>{'Kommentar: '}</td>
                <td>{t.Kopf.Kommentar}</td>
              </tr>
            </table>
          </div>
        </div>
      );

      s = t.Spiele;
      spielplan = (
        <div className="p-1 mt-1 border border-dark">
          <div className="">
            Spielplan:
          </div>
          <Table
            daten={s}
            cols={[
              { Column: 'Spiel', Label: 'Spiel' },
              { Column: 'Runde', Label: 'Runde' },
              { Column: 'Gruppe', Label: 'Gruppe' },
              { Column: 'GruppenSpiel', Label: 'Gruppenspiel' },
              { Column: 'IstMannA', Label: 'Mannschaft1' },
              { Column: 'IstMannB', Label: 'Mannschaft2' },
              { Column: 'ToreA', Label: 'Tore1' },
              { Column: 'ToreB', Label: 'Tore2' },
            ]}
          />
        </div>
      );

      g = (
        <div className="p-1 mt-1 border border-dark">
          <div>
            Diagramm:
          </div>
          <Graphviz
            dot={GraphCalc(t)}
            fit="true"
            height="100"
            width="200"
            zoom="true"
          />
        </div>
      );
    }

    return (
      <div className="border border-fsc">
        <div className="d-flex flex-column">
          <div className="text-white bg-secondary pl-1 pr-1">
            Systemdetails
          </div>
          {tInfo}
          {spielplan}
          {g}
        </div>
      </div>
    );
  }
}

SystemDetails.propTypes = {
  system: PropTypes.objectOf(ClsTurnier),
};

SystemDetails.defaultProps = {
  system: null,
};

export default SystemDetails;

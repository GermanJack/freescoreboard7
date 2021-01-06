import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { Graphviz } from 'graphviz-react';
import GraphCalc from '../GraphCalc';
import ClsTurnier from '../dataClasses/ClsTurnier';
import Table from '../../StdComponents/Table';
import '../../print.css';

class TurnierDetails extends Component {
  constructor(props) {
    super(props);
    this.state = {
    };
  }

  render() {
    let t = null;
    let s = [];
    let e = [];
    let tab = [];

    // Anzeigeblöcke
    let g = null;
    let tInfo = '';
    let spielplan = null;
    let tabellen = null;
    let ereignisse = null;
    let torjaeger = null;
    let status = '';

    // alle Berechnungen nur wenn Turnier gewählt
    if (this.props.turnier !== null) {
      // eslint-disable-next-line prefer-destructuring
      t = this.props.turnier[0];
      if (t.Kopf.status === 0) {
        status = 'neu';
      } else if (t.Kopf.status === 2) {
        status = 'gestartet';
      } else if (t.Kopf.status === 3) {
        status = 'beendet';
      }

      // Turnierbeschreibung
      if (this.props.objekte.find((x) => x.Name === 'Info').sichtbar) {
        tInfo = (
          <div className="p-1 mt-1 border border-dark">
            <div>
              Turnierinfos:
            </div>
            <div className="w-50">
              <table className="table table-sm table-borderless">
                <tr className="">
                  <td>{`${t.Kopf.Turniertyp}-Nr.: `}</td>
                  <td>{t.Kopf.ID}</td>
                  <td>{status}</td>
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
      }

      if (this.props.objekte.find((x) => x.Name === 'Spielplan').sichtbar) {
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
      }

      if (this.props.objekte.find((x) => x.Name === 'Tabellen').sichtbar) {
        tab = t.Tabellen;
        const sortBy = [
          { prop: 'Runde', direction: 1 },
          { prop: 'Gruppe', direction: 1 },
          { prop: 'Platz', direction: 1 },
        ];
        tab.sort((a, b) => {
          let i = 0;
          let result = 0;
          while (i < sortBy.length && result === 0) {
            result = sortBy[i].direction * (
              // eslint-disable-next-line no-nested-ternary
              a[sortBy[i].prop].toString() < b[sortBy[i].prop].toString() ? -1
                : (a[sortBy[i].prop].toString() > b[sortBy[i].prop].toString() ? 1 : 0)
            );
            i += 1;
          }
          return result;
        });

        tabellen = (
          <div className="p-1 mt-1 border border-dark">
            <div className="">
              Tabellen:
            </div>
            <Table
              daten={tab}
              cols={[
                { Column: 'Runde', Label: 'Runde' },
                { Column: 'Gruppe', Label: 'Gruppe' },
                { Column: 'Platz', Label: 'Platz' },
                { Column: 'Mannschaft', Label: 'Mannschaft' },
                { Column: 'Punkte', Label: 'Punkte' },
                { Column: 'Tore', Label: 'Tore' },
                { Column: 'Gegentore', Label: 'Gegentore' },
              ]}
            />
          </div>
        );
      }

      if (this.props.objekte.find((x) => x.Name === 'Ereignisse').sichtbar) {
        e = t.Ereignisse;
        ereignisse = (
          <div className="p-1 mt-1 border border-dark">
            <div className="">
              Ereignisse:
            </div>
            <Table
              daten={e}
              cols={[
                { Column: 'Spiel', Label: 'Spiel' },
                { Column: 'Spielzeit', Label: 'Spielzeit' },
                { Column: 'Ereignistyp', Label: 'Ereignistyp' },
                { Column: 'Mannschaft', Label: 'Mannschaft' },
                { Column: 'Spieler', Label: 'Spieler' },
              ]}
            />
          </div>
        );
      }

      if (this.props.objekte.find((x) => x.Name === 'Torjäger').sichtbar) {
        const h = t.Torschuetzen;
        torjaeger = (
          <div className="p-1 mt-1 border border-dark">
            <div>
              Torschützen:
            </div>
            <div>
              <Table
                daten={h}
                cols={[
                  { Column: 'Mannschaft', Label: 'Mannschaft' },
                  { Column: 'Spieler', Label: 'Spieler' },
                  { Column: 'Tore', Label: 'Tore' },
                ]}
              />
            </div>
          </div>
        );
      }

      if (this.props.objekte.find((x) => x.Name === 'Diagramm').sichtbar) {
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
    }

    return (
      <div className="border border-fsc">
        <div className="d-flex flex-column">
          <div className="text-white bg-secondary pl-1 pr-1">
            Turnierbericht
          </div>
          {tInfo}
          {spielplan}
          {tabellen}
          {ereignisse}
          {torjaeger}
          <div className="page-break">Test</div>
          {g}
        </div>
      </div>
    );
  }
}

TurnierDetails.propTypes = {
  turnier: PropTypes.objectOf(ClsTurnier),
  objekte: PropTypes.arrayOf(PropTypes.object),
};

TurnierDetails.defaultProps = {
  turnier: null,
  objekte: [],
};

export default TurnierDetails;

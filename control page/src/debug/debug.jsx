import React, { Component } from 'react';
import PropTypes from 'prop-types';
import Table from '../StdComponents/Table';

class Debug extends Component {
  setDefault(ID, e) {
    this.props.websocketSend({ Command: 'SetVariableDefault', Value1: ID, Value2: e.target.value });
  }

  switchHeartBeat(e) {
    this.props.websocketSend({ Command: 'SetHeartBeatStatus', Value1: e.target.checked });
  }

  render() {
    let hs = false;
    if (this.props.HeartBeatStatus === 'True') {
      hs = true;
    } else {
      hs = false;
    }

    const textItem = this.props.textvariablen.map((x) => {
      let rw = <td />;
      if (x.ID !== 'T00') {
        rw = (
          <input
            type="textbox"
            className="form-control form-control-sm"
            id={x.ID}
            value={x.Default}
            onChange={this.setDefault.bind(this, x.ID)}
          />
        );
      }

      const r = (
        <tr className="pt-0 pb-0">
          <td className="">{x.ID}</td>
          <td className="">{x.Variable}</td>
          <td className="">{x.Wert}</td>
          <td>
            {rw}
          </td>
        </tr>
      );
      return r;
    });

    return (
      <div className="">
        <div className="border">
          <div className="text-center bg-fsc text-light p-1">
            Debug
          </div>

          <div className="d-flex flex-row justify-content-center">
            <div className="pr-2 mr-1">
              HeartBeatStatus (Timer):
            </div>
            <label htmlFor="x" className="switch pl-2 text-center">
              <input type="checkbox" id="x" checked={hs} onChange={this.switchHeartBeat.bind(this)} />
              <span className="slider round" />
            </label>
          </div>

          <div className="d-flex flex-row border-dark border-top">
            <div className="d-flex flex-column p-2">
              <div>
                Text-Variablen:
              </div>
              <div className="p-0 border border-dark" style={{ height: this.props.height, overflowY: 'auto' }}>
                <table className="table table-sm p-0 m-0 bs-0">
                  <thead className="thead-light">
                    <tr className="d-table-row">
                      <td className="">ID</td>
                      <td className="">Variable</td>
                      <td className="">Wert</td>
                      <td className="">Reset-Wert</td>
                    </tr>
                  </thead>
                  <tbody style={{ overflow: 'auto' }}>
                    {textItem}
                  </tbody>
                </table>
              </div>
            </div>
            {/* <div className="d-flex flex-column p-2">
              <div>
                Text-Variablen:
              </div>
              <Table
                daten={this.props.textvariablen}
                cols={[
                  { Column: 'ID', Label: 'ID' },
                  { Column: 'Variable', Label: 'Variable' },
                  { Column: 'Wert', Label: 'Wert' },
                  { Column: 'Default', Label: 'Default' },
                ]}
                chk=""
              />
            </div> */}

            <div className="d-flex flex-column p-2">
              <div>
                Bild-Variablen:
              </div>
              <Table
                daten={this.props.picVariables}
                cols={[{ Column: 'ID', Label: 'ID' }, { Column: 'Variable', Label: 'Variable' }, { Column: 'Wert', Label: 'Wert' }]}
                chk=""
              />
            </div>

          </div>
          <div className="d-flex flex-column p-2">
            <div>
              Tabellen-Variablen:
            </div>
            <Table
              daten={this.props.tableVariables}
              cols={[{ Column: 'ID', Label: 'ID' }, { Column: 'Variable', Label: 'Variable' }, { Column: 'Wert', Label: 'Wert' }]}
              chk=""
            />
          </div>
        </div>
      </div>
    );
  }
}

Debug.propTypes = {
  HeartBeatStatus: PropTypes.bool,
  textvariablen: PropTypes.arrayOf(PropTypes.object),
  picVariables: PropTypes.arrayOf(PropTypes.object),
  tableVariables: PropTypes.arrayOf(PropTypes.object),
  websocketSend: PropTypes.func,
};

Debug.defaultProps = {
  HeartBeatStatus: false,
  textvariablen: [],
  picVariables: [],
  tableVariables: [],
  websocketSend: () => {},
};

export default Debug;

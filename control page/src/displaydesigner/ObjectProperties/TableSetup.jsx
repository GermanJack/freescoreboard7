import React, { Component } from 'react';
import PropTypes from 'prop-types';
import Dropdown from '../../StdComponents/Dropdown';

class TableSetup extends Component {
  constructor(props) {
    super(props);
    this.state = {
    };
  }

  handleVarSelection(e) {
    if (e !== '[keine dyn. Tabelle]') {
      const vl = this.props.tableVariables;
      const vi = vl.findIndex((x) => x.Variable === e);
      const v = this.props.tableVariables[vi];
      this.props.onAtributeChange('tableid', v.ID);
    } else {
      this.props.onAtributeChange('tableid', 'T00');
    }
  }

  handleStyleSelection(e) {
    this.props.onAtributeChange('TableStyle', e);
  }

  render() {
    const vl = [];
    let tableidtxt = '[keine dyn. Tabelle]';
    this.props.tableVariables.sort((a, b) => ((a.Variable > b.Variable) ? 1 : ((b.Variable > a.Variable) ? -1 : 0)));
    for (let i = 0; i < this.props.tableVariables.length; i += 1) {
      const v = this.props.tableVariables[i];
      vl.push(v.Variable);

      if (v.ID === this.props.tablevariable) {
        tableidtxt = v.Variable;
      }
    }

    return (
      <div className="border border-dark">
        <div className="text-white bg-secondary pl-1 pr-1">Tabellen</div>

        <div className="d-flex flex-row pt-1 pl-1 pr-1">
          {/*  TabellenVariable */}
          <Dropdown
            className=""
            toolTip="dynamische Tabelle"
            data-toggle="tooltip"
            data-placement="right"
            title="dyn. Tabelle"
            values={vl}
            value={tableidtxt}
            wahl={this.handleVarSelection.bind(this)}
          />
        </div>

        <div className="d-flex flex-row pt-1 pl-1 pr-1">
          {/*  TabellenStyle */}
          <Dropdown
            className=""
            toolTip="TabellenStyle"
            data-toggle="tooltip"
            data-placement="right"
            title="Style"
            values={['Standard', 'EinfachBlau', 'EinfachGruen', 'Schwarz', 'Weis', 'ZeilenBlau', 'ZeilenGruen']}
            value={this.props.tablestyle}
            wahl={this.handleStyleSelection.bind(this)}
          />
        </div>
      </div>
    );
  }
}

TableSetup.propTypes = {
  tableVariables: PropTypes.arrayOf(PropTypes.object).isRequired,
  tablevariable: PropTypes.string.isRequired,
  tablestyle: PropTypes.string.isRequired,
  onAtributeChange: PropTypes.func.isRequired,
};

export default TableSetup;

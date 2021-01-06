import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { IoMdArrowDropdown } from 'react-icons/io';
import DlgTable from '../../StdComponents/DlgTable';
import '../../App.css';

class Mannschaftsselector extends Component {
  handleManWahl(e) {
    this.props.onMannWahl(e.wahl);
  }

  handleManualEntry(e) {
    this.props.onNameTyped(e);
  }

  render() {
    const ml = [];
    for (let i = 0; i < this.props.teamList.length; i += 1) {
      ml.push({ ID: this.props.teamList[i].ID, Name: this.props.teamList[i].Name });
    }

    const titel = `${this.props.Titel} wählen:`;

    let anz = null;
    if (this.props.modus === 'Standard') {
      anz = (
        <div className="input-group">
          <input
            type="text"
            className="form-control"
            value={this.props.mannschaft}
            spellCheck="false"
            onChange={this.handleManualEntry.bind(this)}
            disabled={this.props.disable}
          />
          <DlgTable
            label1={titel}
            text=""
            className=""
            data-toggle="tooltip"
            data-placement="right"
            toolTip="Mannschaft wählen"
            reacticon={<IoMdArrowDropdown size="1.5em" />}
            modalID={this.props.Titel}
            data={ml}
            wahl={this.handleManWahl.bind(this)}
            disabled={this.props.disable}
          />
        </div>
      );
    } else {
      anz = <div className="d-flex pl-1 border border-dark rounded justify-content-center">Team Name</div>;
    }

    return (
      <div className="text-left">
        <div className="pl-1">
          {`${this.props.Titel} :`}
        </div>
        {anz}

      </div>
    );
  }
}

Mannschaftsselector.propTypes = {
  teamList: PropTypes.arrayOf(PropTypes.object).isRequired,
  Titel: PropTypes.string.isRequired,
  modus: PropTypes.string.isRequired,
  mannschaft: PropTypes.string.isRequired,
  onMannWahl: PropTypes.func.isRequired,
  onNameTyped: PropTypes.func.isRequired,
  disable: PropTypes.bool,
};

Mannschaftsselector.defaultProps = {
  disable: false,
};

export default Mannschaftsselector;

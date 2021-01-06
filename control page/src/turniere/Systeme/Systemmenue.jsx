import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { GiFamilyTree } from 'react-icons/gi';

import DlgSystemNeu from './DlgSystemNeu';
import DlgWarning from '../../StdComponents/DlgWarning';

class Systemmenue extends Component {
  constructor() {
    super();
    this.state = {
    };

    this.treeRef = React.createRef();
  }

  clickDel() {
    this.props.turnierDelete('System');
  }

  clickGrafik() {
    this.props.grafik();
  }

  render() {
    return (
      <div className="d-flex flex-column">

        <div className="d-flex flex-row p-1">
          <div className="mr-1">
            <DlgSystemNeu
              label1="neues Turniersystem"
              toolTip="neues Turniersystem"
              modalID="Modal-Sys1"
              namensliste={this.props.namensliste}
              websocketSend={this.props.websocketSend.bind(this)}
            />
          </div>

          <div className="mr-1">
            <DlgWarning
              modalID="delsys1"
              label1="Turniersystem unwiederruflich löschen?"
              toolTip="Turniersystem löschen"
              reacticon=""
              btnclassname=""
              name={this.clickDel.bind(this)}
            />
          </div>

          <div className="mr-1">
            <button
              type="button"
              className="btn btn-primary border-dark p-0 pl-1 pr-1 pb-1"
              onClick={this.clickGrafik.bind(this)}
              title="Turniersystem Gerfik"
            >
              <GiFamilyTree />
            </button>
          </div>
        </div>
      </div>
    );
  }
}

Systemmenue.propTypes = {
  namensliste: PropTypes.arrayOf(PropTypes.string),
  websocketSend: PropTypes.func,
  turnierDelete: PropTypes.func,
  grafik: PropTypes.func,
};

Systemmenue.defaultProps = {
  namensliste: [],
  websocketSend: () => {},
  turnierDelete: () => {},
  grafik: () => {},
};

export default Systemmenue;

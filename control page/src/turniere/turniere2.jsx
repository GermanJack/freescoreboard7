import React, { Component } from 'react';
import PropTypes from 'prop-types';
import TurnirList from './Turniere/TurnierList';
import SystemList from './Systeme/SystemList';

class Turniere2 extends Component {
  constructor(props) {
    super(props);
    this.state = {
    };
  }

  render() {
    return (

      <div className="">
        <div className="text-center bg-fsc text-light p-1 mb-1">
          Turnierverwaltung
        </div>
        <ul className="nav nav-pills">
          <li className="nav-item">
            <a className="nav-link active" data-toggle="tab" data-labelid="TabSetup" href="#Turniere">Turniere</a>
          </li>

          <li className="nav-item">
            <a className="nav-link" data-toggle="tab" data-labelid="TabBedienung" href="#Systeme">Systeme</a>
          </li>
        </ul>

        <div className="tab-content bg-light mt-1">

          <div className="tab-pane fade show active" id="Turniere">
            <TurnirList
              turniere={this.props.turniere}
              teamList={this.props.teamList}
              activeTurnierID={this.props.activeTurnierID}
              websocketSend={this.props.websocketSend.bind(this)}
            />
          </div>

          <div className="tab-pane fade" id="Systeme">
            <SystemList
              turniere={this.props.turniere}
              teamList={this.props.teamList}
              websocketSend={this.props.websocketSend.bind(this)}
            />
          </div>

        </div>

      </div>
    );
  }
}

Turniere2.propTypes = {
  turniere: PropTypes.arrayOf(PropTypes.arrayOf.objects),
  teamList: PropTypes.arrayOf(PropTypes.object),
  activeTurnierID: PropTypes.number,
  websocketSend: PropTypes.func.isRequired,
};

Turniere2.defaultProps = {
  turniere: [],
  activeTurnierID: 0,
  teamList: [],
};

export default Turniere2;

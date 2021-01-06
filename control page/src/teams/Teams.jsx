import React, { Component } from 'react';
import PropTypes from 'prop-types';
import TeamList from './TeamList';
import TeamDetail from './TeamDetail';
import PlayerList from './PlayerList';
import PlayerDetail from './PlayerDetail';

class Teams extends Component {
  constructor() {
    super();
    this.state = {
      team: '',
      player: '',
    };
  }

  componentDidUpdate(prevProps) {
    if (prevProps.teamList !== this.props.teamList) {
      this.refreschMan();
    }
    if (prevProps.playerList !== this.props.playerList) {
      this.refreschSpieler(prevProps.playerList);
    }
  }

  mannWahl(ID) {
    const ml = this.props.teamList;
    const mi = ml.findIndex((x) => x.ID === ID);
    const m = this.props.teamList[mi];
    this.setState({ team: m });
    this.setState({ player: '' });
    // this.setState({playerList: []});
    const t = m.ID.toString();
    this.props.websocketSend({ Type: 'req', Command: 'PlayerList', Team: t });
  }

  refreschMan() {
    if (!this.state.team) {
      this.mannWahl(this.props.teamList[0].ID);
    } else {
      const ind = this.props.teamList.findIndex((x) => x.ID === this.state.team.ID);
      this.mannWahl(this.props.teamList[ind].ID);
    }
  }

  mannDel(ID) {
    this.setState({ team: '' });
    this.props.websocketSend({ Type: 'bef', Command: 'TeamDel', Team: ID });
  }

  spielrerWahl(ID) {
    const sl = this.props.playerList;
    const si = sl.findIndex((x) => x.ID === ID);
    const s = this.props.playerList[si];
    this.setState({ player: s });
  }

  refreschSpieler(pl) {
    const pid = this.state.player.ID;
    const plids = pl.map((a) => a.ID);
    if (pid) {
      // check if selected player is still in list
      // when a player is deleted or moved to anothe team this is not the case any more
      // if selected player is not found then delete selection
      if (!plids.includes(pid)) {
        this.setState({ player: null }, () => { this.selectfirstplayer(); });
      } else {
        this.spielrerWahl(pid);
      }
    } else {
      this.selectfirstplayer();
    }
  }

  selectfirstplayer() {
    if (this.props.playerList.length > 0) {
      const c = this.props.playerList[0].ID;
      this.spielrerWahl(c);
    }
  }

  playerDel(ID) {
    this.setState({ player: '' });
    this.props.websocketSend({
      Type: 'bef',
      Command: 'PlayerDel',
      Team: this.state.team.ID,
      Player: ID,
    });
  }

  render() {
    return (
      <div className="d-flex flex-column">

        <div className="text-center bg-fsc text-light p-1">
          Mannschaftenverwaltung
        </div>

        <div className="d-flex flex-row flex-grow-1">

          <div className="d-flex border border-fsc">

            <TeamList
              teamList={this.props.teamList}
              team={this.state.team}
              objektWahl={this.mannWahl.bind(this)}
              onDel={this.mannDel.bind(this)}
              websocketSend={this.props.websocketSend.bind(this)}
              websocketSendRaw={this.props.websocketSendRaw.bind(this)}
            />
          </div>

          <div className="d-flex flex-column flex-grow-1">
            <div className="d-flex flex-row border border-fsc">
              <TeamDetail
                team={this.state.team}
                picList={this.props.picList}
                websocketSend={this.props.websocketSend.bind(this)}
              />
            </div>

            <div className="d-flex flex-row flex-grow-1">
              <div className="d-flex flex-column border border-fsc">

                <PlayerList
                  teamList={this.props.teamList}
                  team={this.state.team}
                  playerList={this.props.playerList}
                  player={this.state.player}
                  onDel={this.playerDel.bind(this)}
                  objektWahl={this.spielrerWahl.bind(this)}
                  websocketSend={this.props.websocketSend.bind(this)}
                />
              </div>

              <div className="d-flex flex-column flex-grow-1 border border-fsc">

                <PlayerDetail
                  player={this.state.player}
                  team={this.state.team}
                  picList={this.props.picList}
                  websocketSend={this.props.websocketSend.bind(this)}
                />
              </div>
            </div>
          </div>
        </div>
      </div>
    );
  }
}

Teams.propTypes = {
  teamList: PropTypes.oneOfType(PropTypes.object),
  playerList: PropTypes.oneOfType(PropTypes.object),
  picList: PropTypes.oneOfType(PropTypes.string),
  websocketSend: PropTypes.func,
  websocketSendRaw: PropTypes.func,
};

Teams.defaultProps = {
  teamList: [],
  playerList: [],
  picList: [],
  websocketSend: () => {},
  websocketSendRaw: () => {},
};

export default Teams;

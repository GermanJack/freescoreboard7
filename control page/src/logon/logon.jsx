import React, { Component } from 'react';
import PropTypes from 'prop-types';

class Logon extends Component {
  constructor(props) {
    super(props);
    this.state = {
      pwd: '',
    };
  }

  entry(e) {
    this.setState({ pwd: e.target.value });
  }

  logon() {
    // console.log(this.state.pwd);
    this.props.websocketSend({ Type: 'logon', Value1: this.state.pwd });
  }

  render() {
    return (
      <div className="">
        <div className="border">
          <div>
            Logon
          </div>
          <form>
            {/* Benutzername: <input type="text" name="email" /> */}
            Password:
            <input
              type="password"
              name="pwd"
              onChange={this.entry.bind(this)}
              className="form-control"
            />
            <button
              type="button"
              onClick={this.logon.bind(this)}
              className="btn btn-secondary"
            >
              Senden
            </button>
          </form>

        </div>
      </div>
    );
  }
}

Logon.propTypes = {
  websocketSend: PropTypes.func,
};

Logon.defaultProps = {
  websocketSend: () => {},
};

export default Logon;

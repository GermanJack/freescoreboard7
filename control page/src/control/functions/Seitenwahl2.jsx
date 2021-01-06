import React, { Component } from 'react';
import PropTypes from 'prop-types';

class Seitenwahl2 extends Component {
  constructor(props) {
    super(props);
    this.Name = {
      value: null,
    };

    // This binding is necessary to make `this` work in the callback
    this.handleClick = this.handleClick.bind(this);
  }

  handleClick(e) {
    if (e === 'X') {
      this.props.websocketSend({ Type: 'bef', Command: 'ToogleDisplay' });
    } else {
      this.props.websocketSend({ Type: 'bef', Command: 'SwitchPage', Page: e });
    }
  }

  render() {
    let item = null;
    if (this.props.pages) {
      // eslint-disable-next-line arrow-body-style
      item = this.props.pages.map((i) => {
        return (
          <button
            type="button"
            className="btn btn-primary border-dark mr-2 mb-1"
            key={i.ID}
            data-dismiss="modal"
            onClick={this.handleClick.bind(this, i.ID)}
          >
            {i.PageName}
          </button>
        );
      });
    }

    return (
      <div className="ml-1 mt-1">
        <div className="row ml-1">
          {item}
          <button
            type="button"
            className="btn btn-primary border-dark mb-1"
            data-labelid="BtnX"
            title="lokale Anzeige ein/aus"
            onClick={this.handleClick.bind(this, 'X')}
          >
            X
          </button>
        </div>
      </div>
    );
  }
}

Seitenwahl2.propTypes = {
  pages: PropTypes.arrayOf(PropTypes.object).isRequired,
  websocketSend: PropTypes.func.isRequired,
};

export default Seitenwahl2;

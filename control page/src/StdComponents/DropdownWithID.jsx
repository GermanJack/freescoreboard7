import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { MdArrowDropDown, MdArrowDropUp } from 'react-icons/md';

// erwartet eine Liste von Objekten mit den Feldern ID un Text: values:{ID:1, Text:"text"}
// zurückgegeben wird nur die ID
// der wert (value) wird über die ID gesetzt

class DropdownWithID extends Component {
  handleclick(par) {
    this.props.wahl(par);
  }

  render() {
    let items = null;
    let selitem = '';
    if (this.props.values) {
      items = this.props.values.map((i) => { // eslint-disable-line arrow-body-style
        return (
          <button
            type="button"
            className="dropdown-item p-0 pl-1 pr-1"
            ID={i.ID}
            onClick={this.handleclick.bind(this, i.ID)}
          >
            {i.Text}
          </button>
        );
      });

      const setitem = this.props.values.find((x) => x.ID === this.props.value);
      if (setitem) {
        selitem = setitem.Text;
      }
    }

    let cName = 'btn btn-outline-secondary btn-sm';
    if (this.props.cName !== '') {
      cName = this.props.cName;
    }

    let direction = 'dropdown';
    let arrow = <MdArrowDropDown size="1.2em" />;
    if (this.props.direction === 'up') {
      direction = 'dropup';
      arrow = <MdArrowDropUp size="1.2em" />;
    }

    return (
      <div className="">
        <div className={direction}>
          <button
            className={cName}
            type="button"
            data-toggle="dropdown"
            aria-haspopup="true"
            aria-expanded="false"
          >
            <div className="d-flex flex-row justify-content-between">
              {selitem}
              {arrow}
            </div>
          </button>
          <div className="dropdown-menu scroll-50">
            {items}
          </div>
        </div>
      </div>
    );
  }
}

DropdownWithID.propTypes = {
  values: PropTypes.arrayOf(PropTypes.object),
  value: PropTypes.string,
  cName: PropTypes.string,
  direction: PropTypes.string,
  wahl: PropTypes.func,
};

DropdownWithID.defaultProps = {
  values: [],
  value: '',
  cName: '',
  direction: 'down',
  wahl: () => {},
};

// values: {ID: 'id', Text: 'text'}, {...}

export default DropdownWithID;

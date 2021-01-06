/* eslint-disable jsx-a11y/label-has-associated-control */
import React from 'react';
import PropTypes from 'prop-types';
import { BiPrinter } from 'react-icons/bi';
import { GoSync } from 'react-icons/go';
import ReactToPrint, { PrintContextConsumer } from 'react-to-print';
import TurnierDetails from './TurnierDetails';
import ClsTurnier from '../dataClasses/ClsTurnier';
import DlgBerischtssetup from '../../StdComponents/DlgBerichtssetup';
import '../../print.css';

class TurnierDetailsPrint extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      objekte: [
        { Name: 'Info', sichtbar: true },
        { Name: 'Spielplan', sichtbar: true },
        { Name: 'Tabellen', sichtbar: true },
        { Name: 'Ereignisse', sichtbar: true },
        { Name: 'Torjäger', sichtbar: true },
        { Name: 'Diagramm', sichtbar: true },
      ],
    };
  }

  changeState(e, f) {
    const obj = {};
    obj[e] = f.target.checked;
    this.setState(obj);
  }

  handleRefresh() {
    this.props.websocketSend({ Type: 'req', Command: 'TurniereKomplett' });
  }

  render() {
    return (
      <div>
        <div className="d-flex flex-row mb-1">
          <ReactToPrint content={() => this.componentRef}>
            <PrintContextConsumer>
              {({ handlePrint }) => (
                <button
                  type="button"
                  onClick={handlePrint}
                  className="btn btn-outline-secondary btn-sm mr-1"
                  title="Turnierbericht drucken"
                >
                  <BiPrinter size="1.2em" />
                </button>
              )}
            </PrintContextConsumer>
          </ReactToPrint>
          <div>
            <DlgBerischtssetup
              werte={this.state.objekte}
              label1="Bereiche einstellen"
            />
          </div>
          <div>
            <button
              type="button"
              onClick={this.handleRefresh.bind(this)}
              className="btn btn-outline-secondary btn-sm mr-1"
              title="refresh"
            >
              <GoSync size="1.2em" />
            </button>
          </div>
        </div>
        <div>
          <TurnierDetails
            // eslint-disable-next-line no-return-assign
            ref={(el) => (this.componentRef = el)}
            turnier={this.props.turnier}
            objekte={this.state.objekte}
          />
        </div>
      </div>
    );
  }
}

TurnierDetailsPrint.propTypes = {
  turnier: PropTypes.objectOf(ClsTurnier).isRequired,
  websocketSend: PropTypes.func.isRequired,
};

export default TurnierDetailsPrint;

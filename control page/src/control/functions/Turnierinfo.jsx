import React from 'react';

function Turnierinfo(R, G, S) {
  return (
    <div className="d-inline pl-1 pr-1 bg-light text-black border border-dark rounded flex-warp" id="LblAktuelleRunde">
      {`R : ${R} | G : ${G} | S : ${S}`}
    </div>
  );
}

export default Turnierinfo;

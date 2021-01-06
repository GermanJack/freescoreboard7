import ClsQuellPlaetze from './dataClasses/Helper1';
import TSpiele from './dataClasses/TSpiele';

// function MannSchonEingruppiert(tabellen, mann) {
//   const ml = tabellen.find((x) => x.Mannschaft === mann);
//   if (ml !== null) {
//     return true;
//   }

//   return false;
// }

function MannschaftenFuellen(teamscope, runden, rundeNr, tabellen) {
  const list = [];
  // this.DgvMannschaften.Rows.Clear();
  const rundennr = parseInt(rundeNr, 10);

  if (teamscope === '1' || teamscope === '2') {
    let ml = [];
    if (teamscope === '1') {
      // nur Mannschaften der Vorrunde
      ml = tabellen.filter((x) => x.Runde === rundennr - 1);
    }

    if (teamscope === '2') {
      // Mannschaften aller Vorrunden
      ml = tabellen.filter((x) => x.Runde < rundennr);
    }

    let i = 1;
    let oldgrp = '';
    for (let j = 0; j < ml.length; j += 1) {
      if (ml[j].Gruppe !== oldgrp) {
        i = 1;
      }

      const text = `${i}ter-${ml[j].Gruppe}`;
      // eslint-disable-next-line no-loop-func
      if (!tabellen.find((x) => x.Mannschaft === text)) {
        const h = new ClsQuellPlaetze();
        h.ID = j;
        h.Quelltyp = teamscope;
        h.QuellGruppe = ml[j].Gruppe;
        h.QuellGruppenplatz = i;
        h.Text = text;
        list.push(h);
      }

      oldgrp = ml[j].Gruppe;
      i += 1;
    }
  }

  if (teamscope === '3') {
    // bester n -te der Vorrunde
    // this.myQuellPlaetze.Clear();

    const runde = rundennr - 1;
    const gruppe = `${runde}A`;

    const manlist = tabellen.filter((x) => x.Runde === runde && x.Gruppe === gruppe);
    const anzman = manlist.length;

    const grplist = runden.find((rx) => rx.Runde === runde);
    const anzgrp = grplist.AnzGrp;

    for (let i = 1; i <= anzgrp; i += 1) {
      for (let j = 1; j <= anzman; j += 1) {
        const text = `${i}ter-${j}ter-Runde${runde}`;
        if (!tabellen.find((x) => x.Mannschaft === text)) {
          const h = new ClsQuellPlaetze();
          h.ID = list.length;
          h.Quelltyp = teamscope;
          h.QuellGruppe = i;
          h.QuellGruppenplatz = j;
          h.Text = text;
          list.push(h);
        }
      }
    }
  }

  return list;
}

function GruppenFuellen(tabellen, runde) {
  const rundennr = parseInt(runde, 10);
  const list = [];
  const ml = tabellen.filter((x) => x.Runde === rundennr);

  for (let i = 0; i < ml.length; i += 1) {
    // list.push({ Mannschaft: ml[i].Mannschaft, Gruppe: ml[i].Gruppe });
    list.push(ml[i]);
  }

  return list;
}

function OddorEven(intNumber) {
  let erg = 'Odd';
  if (intNumber % 2 === 0) {
    erg = 'Even';
  }

  return erg;
}

function JGJgruppe(runde, grp, mann) {
  const gSpielplan = [];
  let mA = ''; // gewählte mannschaftA
  let mB = ''; // gewählte mannschaftB

  let n = 0; // Anzahl Mannschaften
  let r = 0; // spiele je gruppe
  let k = 0; // paarungen je gruppe
  let a = 0; // ergebnis für Mannschaft 1
  let b = 0; // ergebnis für Mannschaft 2
  let s = 0; // spielNr

  if (OddorEven(mann.length) === 'Even') {
    n = mann.length; // gerade Anz. Mannschaften
  } else {
    n = mann.length + 1; // ungerade Anz. Mannschaften
  }

  for (r = 1; r <= n - 1; r += 1) {
    // erste Paarung der gruppe
    // nur bei geraden Anz. mannschaften
    if (OddorEven(mann.length) === 'Even') {
      // grp.ListIndex = (i - 1)
      mA = mann[r - 1].toString();
      mB = mann[n - 1].toString();

      s += 1;
      const s1 = new TSpiele();
      s1.Spiel = s;
      s1.Runde = runde;
      s1.Gruppe = grp;
      s1.MannA = mA;
      s1.MannB = mB;

      gSpielplan.push(s1);
    }

    // 2te und weitere paarungen
    // -1 da erste paarung schon oben
    for (k = 1; k <= (n / 2) - 1; k += 1) {
      a = (r + k) % (n - 1);
      if (a === 0) {
        a = n - 1;
      }

      b = (r - k) % (n - 1);
      if (b === 0) {
        b = n - 1;
      }

      if (b < 0) {
        b = n - 1 + b;
      }

      // wenn wegen ungerade Anz. Mannschaften ungültiges spiel
      if (a <= mann.length || b <= mann.length) {
        // grp.ListIndex = (i - 1)
        mA = mann[a - 1].toString();
        mB = mann[b - 1].toString();
        if (mA !== mB) {
          s += 1;
          const s2 = new TSpiele();
          s2.Spiel = s;
          s2.Runde = runde;
          s2.Gruppe = grp;
          s2.MannA = mA;
          s2.MannB = mB;

          gSpielplan.push(s2);
        }
      }
    }
  }

  return gSpielplan;
}

function jgjRunde(runde, gruppen, tabellen) {
  let rundenPlan = [];

  for (let i = 0; i < gruppen.length; i += 1) {
    const ml = tabellen.filter(
      (x) => x.Gruppe === gruppen[i],
    ).map((y) => y.Mannschaft);
    let grpPlan = [];
    grpPlan = JGJgruppe(runde, gruppen[i], ml);
    rundenPlan = rundenPlan.concat(grpPlan);
  }

  return rundenPlan;
}

function jgjTurnier(tabellen) {
  let Plan = [];
  const rl = tabellen.map((x) => x.Runde);
  const distinctRl = [...new Set(rl)];
  for (let i = 0; i < distinctRl.length; i += 1) {
    const ml = tabellen.filter(
      (x) => x.Runde === distinctRl[i],
    );
    const gl = tabellen.filter((x) => x.Runde === distinctRl[i]).map((y) => y.Gruppe);
    const distinctGl = [...new Set(gl)];
    let rundenPlan = [];
    rundenPlan = jgjRunde(distinctRl[i], distinctGl, ml);
    Plan = Plan.concat(rundenPlan);
  }

  return Plan;
}


function PlatzschonDa(spiele, platz) {
  if (spiele.filter((x) => x.SPlatz === platz).length > 0) {
    return 1;
  }

  if (spiele.filter((x) => x.VPlatz === platz).length > 0) {
    return 1;
  }

  return 0;
}

function calcPlaetze(mananz, spiele) {
  const list = [];
  for (let i = 1; i <= mananz; i += 1) {
    if (PlatzschonDa(spiele, i) === 0) {
      list.push(i);
    }
  }
  return list;
}

/**
 * Sorts an array of objects by column/property.
 * @param {Array} array - The array of objects.
 * @param {object} sortObject - The object that contains the sort order keys with directions (asc/desc). e.g. { age: 'desc', name: 'asc' }
 * @returns {Array} The sorted array.
 */
function multiSort(array, sortObject = {}) {
  const sortKeys = Object.keys(sortObject);

  // Return array if no sort object is supplied.
  if (!sortKeys.length) {
    return array;
  }

  // Change the values of the sortObject keys to -1, 0, or 1.
  // for (const key in sortObject) {
  //   sortObject[key] = sortObject[key] === 'desc'
  //   || sortObject[key] === -1 ? -1
  //     : (sortObject[key] === 'skip' || sortObject[key] === 0 ? 0 : 1);
  // }

  for (const key in sortObject) {
    if (sortObject[key] === 'desc' || sortObject[key] === -1) {
      sortObject[key] = -1;
    } else if (sortObject[key] === 'skip' || sortObject[key] === 0) {
      sortObject[key] = 0;
    } else {
      sortObject[key] = 1;
    }
  }


  const keySort = (a, b, direction) => {
    const direction1 = direction !== null ? direction : 1;

    if (a === b) { // If the values are the same, do not switch positions.
      return 0;
    }

    // If b > a, multiply by -1 to get the reverse direction.
    return a > b ? direction1 : -1 * direction1;
  };

  return array.sort((a, b) => {
    let sorted = 0;
    let index = 0;

    // Loop until sorted (-1 or 1) or until the sort keys have been processed.
    while (sorted === 0 && index < sortKeys.length) {
      const key = sortKeys[index];

      if (key) {
        const direction = sortObject[key];
        sorted = keySort(a[key], b[key], direction);
        index += 1;
      }
    }

    return sorted;
  });
}

// eslint-disable-next-line import/prefer-default-export
export { MannschaftenFuellen };
export { GruppenFuellen };
export { JGJgruppe };
export { jgjRunde };
export { jgjTurnier };
export { calcPlaetze };
export { multiSort };
// export { MannSchonEingruppiert };

import React, { useState, useEffect } from "react";

import { XYPlot, XAxis, YAxis, LineSeries, Crosshair } from "react-vis";

const formatCrosshairTitle = values => {
   return {
      title: "Year",
      value: values[0].x
   };
};

const formatCrosshairItems = values => {
   return values.map(v => {
      return {
         title: "Bachelor Degrees Awarded",
         value: v.y
      };
   });
};

const LineChart = props => {
   const [crosshairValues, setCrosshairValues] = useState([]);

   const schoolNames = props.Education.ActiveSchools.map(school => school.name);
   useEffect(() => props.updateDegreesData(schoolNames), []);

   const nearestXHandler = (value, { index }) => {
      setCrosshairValues([props.Education.ActiveSchools[0][index]]);
   };

   const mouseLeaveHandler = () => {
      setCrosshairValues([]);
   };

   const lineChartWidth = 500;
   const lineChartHeight = 300;

   const dataPoints =
      props.Education.DegreesByYear.length > 0
         ? props.Education.DegreesByYear[0]
         : [];
   return (
      <div>
         <h4>{props.Education.ActiveSchools[0].name}</h4>
         <XYPlot
            onMouseLeave={mouseLeaveHandler}
            width={lineChartWidth}
            height={lineChartHeight}
            yDomain={[0, Math.max(...dataPoints.map(point => point.y))]}
            margin={{ left: 64 }}
         >
            <XAxis
               title="Year"
               tickValues={[2012, 2013, 2014, 2015, 2016]}
               tickFormat={v => v.toString()}
            />
            <YAxis title="Number of Bachelors Degrees" />

            <LineSeries
               animation={"noWobble"}
               data={dataPoints}
               curve={"curveCatmullRom"}
               color="blue"
               style={{ strokeWidth: 3 }}
               onNearestX={nearestXHandler}
            />

            <Crosshair
               values={crosshairValues}
               titleFormat={formatCrosshairTitle}
               itemsFormat={formatCrosshairItems}
            />
         </XYPlot>
      </div>
   );
};

export default LineChart;

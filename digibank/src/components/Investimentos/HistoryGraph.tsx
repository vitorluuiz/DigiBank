import React, { useMemo, useCallback } from 'react';
import { AreaClosed, Line, Bar } from '@visx/shape';
import { curveMonotoneX } from '@visx/curve';
import { GridRows, GridColumns } from '@visx/grid';
import { scaleTime, scaleLinear } from '@visx/scale';
import { withTooltip, Tooltip, TooltipWithBounds, defaultStyles } from '@visx/tooltip';
import { localPoint } from '@visx/event';
import { LinearGradient } from '@visx/gradient';
// eslint-disable-next-line @typescript-eslint/no-unused-vars
import { max, extent, bisector, min } from 'd3-array'; // Changed from '@visx/vendor/d3-array'
import { timeFormat } from 'd3-time-format'; // Changed from '@visx/vendor/d3-time-format'
import { HistoryOptionProps } from '../../@types/HistoryOption';

type TooltipData = HistoryOptionProps;

type AreaProps = {
  width: number;
  height: number;
  margin?: { top: number; right: number; bottom: number; left: number };
  historyData: HistoryOptionProps[];
};

export default withTooltip<AreaProps, TooltipData>(
  ({
    width,
    height,
    margin = { top: 0, right: 0, bottom: 0, left: 0 },
    showTooltip,
    hideTooltip,
    tooltipData,
    tooltipTop = 0,
    tooltipLeft = 0,
    historyData,
  }: AreaProps) => {
    if (width < 10) return null;

    const stock = historyData;
    const background = '#ffffff';
    // const background2 = '#c00414';
    // const background2 = '#c00414';
    const accentColor = '#c00414';
    const accentColorDark = '#c00414';
    const tooltipStyles = {
      ...defaultStyles,
      background,
      border: '1px solid white',
      color: 'black',
    };

    // util
    const formatDate = timeFormat("%b %d, '%y");

    // accessors
    const getDate = (d: HistoryOptionProps) => new Date(d.dataH);
    const getStockValue = (d: HistoryOptionProps) => d.valor;
    const bisectDate = bisector<HistoryOptionProps, Date>((d) => new Date(d.dataH)).left;

    // bounds
    const innerWidth = width - margin.left - margin.right;
    const innerHeight = height - margin.top - margin.bottom;

    // scales
    const dateScale = useMemo(
      () =>
        scaleTime({
          range: [margin.left, innerWidth + margin.left],
          domain: extent(stock, getDate) as [Date, Date],
        }),
      [innerWidth, margin.left, stock],
    );
    const stockValueScale = useMemo(
      () =>
        scaleLinear({
          range: [innerHeight + margin.top, margin.top],
          domain: [
            (min(stock, getStockValue) || 0) - ((min(stock, getStockValue) || 0) / 100) * 0.1,
            (max(stock, getStockValue) || 0) + ((max(stock, getStockValue) || 0) / 100) * 0.1,
          ],
          clamp: true,
          nice: true,
        }),
      [innerHeight, margin.top, stock],
    );

    // tooltip handler
    const handleTooltip = useCallback(
      (event: React.TouchEvent<SVGRectElement> | React.MouseEvent<SVGRectElement>) => {
        const { x } = localPoint(event) || { x: 0 };
        const x0 = dateScale.invert(x);
        const index = bisectDate(stock, x0, 1);
        const d0 = stock[index - 1];
        const d1 = stock[index];
        let d = d0;
        if (d1 && getDate(d1)) {
          d = x0.valueOf() - getDate(d0).valueOf() > getDate(d1).valueOf() - x0.valueOf() ? d1 : d0;
        }
        showTooltip({
          tooltipData: d,
          tooltipLeft: x,
          tooltipTop: stockValueScale(getStockValue(d)),
        });
      },
      [dateScale, bisectDate, stock, showTooltip, stockValueScale],
    );

    return (
      <div style={{ width: '100%', height: '100%' }}>
        <svg width="100%" height="100%">
          <rect
            x={0}
            y={0}
            width={width}
            height={height}
            fill="url(#area-background-gradient)"
            rx={14}
          />
          {/* <LinearGradient id="area-background-gradient" from={background} to={background} /> */}
          <LinearGradient id="area-gradient" from={accentColor} to={accentColor} toOpacity={0.6} />
          <GridRows
            left={margin.left}
            scale={stockValueScale}
            width={innerWidth}
            strokeDasharray="1,3"
            stroke={accentColor}
            strokeOpacity={0}
            pointerEvents="none"
          />
          <GridColumns
            top={margin.top}
            scale={dateScale}
            height={innerHeight}
            strokeDasharray="1,3"
            stroke={accentColor}
            strokeOpacity={0.2}
            pointerEvents="none"
          />
          <AreaClosed<HistoryOptionProps>
            data={stock}
            x={(d) => dateScale(getDate(d)) ?? 0}
            y={(d) => stockValueScale(getStockValue(d)) ?? 0}
            yScale={stockValueScale}
            strokeWidth={1}
            stroke="url(#area-gradient)"
            fill="url(#area-gradient)"
            // strokeWidth={4}
            // fill="#000"
            // stroke="#c00414"
            curve={curveMonotoneX}
          />
          <Bar
            x={margin.left}
            y={margin.top}
            width={innerWidth}
            height={innerHeight}
            fill="transparent"
            rx={14}
            onTouchStart={handleTooltip}
            onTouchMove={handleTooltip}
            onMouseMove={handleTooltip}
            onMouseLeave={() => hideTooltip()}
          />
          {tooltipData && (
            <g>
              <Line
                from={{ x: tooltipLeft, y: margin.top }}
                to={{ x: tooltipLeft, y: innerHeight + margin.top }}
                stroke={accentColorDark}
                strokeWidth={2}
                pointerEvents="none"
                strokeDasharray="5,2"
              />
              <circle
                cx={tooltipLeft}
                cy={tooltipTop + 1}
                r={4}
                fill="black"
                fillOpacity={0.1}
                stroke="black"
                strokeOpacity={0.1}
                strokeWidth={2}
                pointerEvents="none"
              />
              <circle
                cx={tooltipLeft}
                cy={tooltipTop}
                r={4}
                fill={accentColorDark}
                stroke="white"
                strokeWidth={2}
                pointerEvents="none"
              />
            </g>
          )}
        </svg>
        {tooltipData && (
          <div>
            <TooltipWithBounds
              key={Math.random()}
              top={tooltipTop - 12}
              left={tooltipLeft + 12}
              style={tooltipStyles}
            >
              {`$${getStockValue(tooltipData)}`}
            </TooltipWithBounds>
            <Tooltip
              top={innerHeight + margin.top - 14}
              left={tooltipLeft}
              style={{
                ...defaultStyles,
                minWidth: 72,
                textAlign: 'center',
                transform: 'translateX(-50%)',
              }}
            >
              {formatDate(getDate(tooltipData))}
            </Tooltip>
          </div>
        )}
      </div>
    );
  },
);

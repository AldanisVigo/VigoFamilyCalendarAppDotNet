import { PieChart } from '@mui/x-charts/PieChart';
import './statchart.css'
const StatChart = ({data}:any) => {
    return <PieChart className="self-center ml-auto mr-auto statchart"
        series={[
            {
            data: data ?? [],
            innerRadius: 30,
            outerRadius: 80,
            paddingAngle: 5,
            cornerRadius: 5,
            startAngle: -90,
            endAngle: 180,
            cx: 100,
            // cy: 94,
            }
        ]}
        slotProps={{
            legend: {
              direction: 'row',
              position: { vertical: 'bottom', horizontal: 'left' },
              padding: 0,
            }
        }}
    />
}

export default StatChart;
import { AppStore, Person } from '@/models';
import { addFavorites } from '@/redux';
import { Checkbox } from '@mui/material';
import { DataGrid, GridRenderCellParams } from '@mui/x-data-grid';
import { FC, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';


export interface PeopleTableInterface {}


const PeopleTable: FC<PeopleTableInterface> = ()=> {

    const pageSize = 5;
    const [selectedItem, setSelectedItem] = useState<Person[]>([]);
    const statedPeople = useSelector((store: AppStore)=> store.people);
    const dispatch = useDispatch();
    const findItem = (person: Person) => !!selectedItem.find((i) => i.id === person.id);
    const filterItem = (person: Person) => selectedItem.filter((i) => i.id !== person.id);
  
    const handleChange = (person: Person) => {
      const filteredItem = findItem(person) ? filterItem(person) : [...selectedItem, person];
      dispatch(addFavorites(filteredItem));
      setSelectedItem(filteredItem);
      console.log(filteredItem);
    };

    const columns = [
      {
        field: 'actions',
        headerName: '',
        type: 'actions',
        minWidth: 50,
        renderCell: (params: GridRenderCellParams) => (
          <>
            {
              <Checkbox
                size='small'
                checked={findItem(params.row)}
                onChange={() => handleChange(params.row)}
              />
            }
          </>
        ),
      },
  
      {
        field: 'name',
        headerName: 'Name',
        flex: 1,
        minWidth: 150,
        renderCell: (params: GridRenderCellParams) => <>{params.value}</>,
      },
      {
        field: 'category',
        headerName: 'Categories',
        flex: 1,
        minWidth: 150,
        renderCell: (params: GridRenderCellParams) => <>{params.value}</>,
      },
      {
        field: 'company',
        headerName: 'Companies',
        flex: 1,
        minWidth: 150,
        renderCell: (params: GridRenderCellParams) => <>{params.value}</>,
      },
    ];
  
    return (
      <div style={{ height: 400, width: '100%' }}>
        <DataGrid
          rows={statedPeople}
          columns={columns}
          getRowId={(row) => row.id}
          disableColumnSelector
          disableRowSelectionOnClick
          autoHeight
          initialState={{
            pagination: {
              paginationModel: { page: 0, pageSize: pageSize },
            },
          }}
          pageSizeOptions={[5, 10]}
        />
      </div>
    );
}

export default PeopleTable;
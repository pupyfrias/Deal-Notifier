/* eslint-disable react-hooks/exhaustive-deps */
import { useDebounce, useFirstRender } from '@/hooks';
import { Brand, Condition, ItemType, OnlineStore, SortBy, UnlockProbability } from '@/models';
import { getBrands, getConditions, getItemTypes, getUnlockProbabilities, banKeywords } from '@/services';
import { getOnlineStores } from '@/services/online-store.service';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import { Button, Checkbox, FormControl, FormControlLabel, FormGroup, Radio, RadioGroup, TextField } from '@mui/material';
import Accordion from '@mui/material/Accordion';
import AccordionDetails from '@mui/material/AccordionDetails';
import AccordionSummary from '@mui/material/AccordionSummary';
import Typography from '@mui/material/Typography';
import { FC, SyntheticEvent, useEffect, useState } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';

const Sidebar: FC = () => {
  const [brands, setBrands] = useState<Brand[]>([]);
  const [conditions, setConditions] = useState<Condition[]>([]);
  const [onlineStores, setOnlineStores] = useState<OnlineStore[]>([]);
  const [itemTypes, setItemTypes] = useState<ItemType[]>([]);
  const [minimum, setMinimum] = useState<string>('');
  const [maximum, setMaximum] = useState<string>('');
  const [unlockProbabilities, setUnlockProbabilities] = useState<UnlockProbability[]>([]);
  const location = useLocation();
  const navigate = useNavigate();
  const debouncedMinimum = useDebounce(minimum, 500);
  const debouncedMaximum = useDebounce(maximum, 500);
  const isFirstRender = useFirstRender();
  const [keyword, setKeyword] = useState('');

  const sortBy: SortBy[] = [
    { name: 'Price: low-high', value: 'orderBy=price' },
    { name: 'Price: high-low', value: 'orderBy=price&descending=true' },
    { name: 'Offer', value: 'OrderBy=saving&Descending=true' },
  ];
  const storages = [32, 64, 128, 256, 512];

  useEffect(() => {
    const fetchData = async () => {
      try {
        const [brandsResponse, unlockProbabilitiesResponse, onlineStoresResponse, conditionsResponse, itemTypesResponse] = await Promise.all([
          getBrands(),
          getUnlockProbabilities(),
          getOnlineStores(),
          getConditions(),
          getItemTypes(),
        ]);

        setBrands(brandsResponse.data.items);
        setUnlockProbabilities(unlockProbabilitiesResponse.data.items);
        setOnlineStores(onlineStoresResponse.data.items);
        setConditions(conditionsResponse.data.items);
        setItemTypes(itemTypesResponse.data.items);
      } catch (error) {
        console.error('Error fetching data:', error);
      }
    };

    fetchData();
  }, []);

  useEffect(() => {
    if (!isFirstRender) {
      const queryParams = new URLSearchParams(location.search);

      if (debouncedMinimum) {
        queryParams.set('min', debouncedMinimum);
      } else {
        queryParams.delete('min');
      }

      console.log('minimum changed');
      const route = `?${queryParams.toString()}`;
      navigate(route);
    }
  }, [debouncedMinimum]);

  useEffect(() => {
    if (!isFirstRender) {
      const queryParams = new URLSearchParams(location.search);

      if (debouncedMaximum) {
        queryParams.set('max', debouncedMaximum);
      } else {
        queryParams.delete('max');
      }

      console.log('maximum changed');
      const route = `?${queryParams.toString()}`;
      navigate(route);
    }
  }, [debouncedMaximum]);

  const handleChangeCheckbox = (event: SyntheticEvent, property: string) => {
    const target = event.target as HTMLInputElement;
    const value = target.value;
    const isChecked = target.checked;
    const queryParams = new URLSearchParams(location.search);
    queryParams.delete('page');

    const propertyQuery = queryParams.get(property);
    let splitPropertyValues: string[] = [];
    let joinedPropertyValue: string = '';

    if (propertyQuery) {
      splitPropertyValues = propertyQuery.split(',');
    }

    if (isChecked) {
      splitPropertyValues.push(value);
      joinedPropertyValue = splitPropertyValues.join(',');
      queryParams.set(property, joinedPropertyValue);
    } else {
      splitPropertyValues = splitPropertyValues.filter((i) => i !== value);
      joinedPropertyValue = splitPropertyValues.join(',');

      if (splitPropertyValues.length > 0) {
        queryParams.set(property, joinedPropertyValue);
      } else {
        queryParams.delete(property);
      }
    }

    const route = queryParams.size > 0 ? `?${queryParams.toString()}` : '/';
    navigate(route);
  };

  const handleChangeRadioButton = (event: SyntheticEvent) => {
    const queryParams = new URLSearchParams(location.search);
    const target = event.target as HTMLInputElement;
    const value = target.value;

    queryParams.delete('orderBy');
    queryParams.delete('descending');
    const route = queryParams.size > 0 ? `?${queryParams.toString()}&${value}` : `?${value}`;
    navigate(route);
  };

  const handleChangeMinimum = (event: SyntheticEvent) => {
    const target = event.target as HTMLInputElement;
    const value = target.value;
    setMinimum(value);
  };

  const handleChangeMaximum = (event: SyntheticEvent) => {
    const target = event.target as HTMLInputElement;
    const value = target.value;
    setMaximum(value);
  };


  const handleBanKeywordChange = (event: SyntheticEvent) => {
    const target = event.target as HTMLInputElement;
    const value = target.value;
    setKeyword(value);
  };


  const handleBanKeyword = async () => {
    
    await banKeywords(keyword);
    setKeyword('');
    navigate('/');
  };

  return (
    <div style={{ margin: '10px 0px 10px 10px' }}>
      <Accordion>
        <AccordionSummary
          expandIcon={<ExpandMoreIcon />}
          aria-controls='panel1bh-content'
          id='panel1bh-header'
        >
          <Typography>Sort by</Typography>
        </AccordionSummary>

        <AccordionDetails>
          <FormControl>
            <RadioGroup
              defaultValue='female'
              name='radio-buttons-group'
              onChange={handleChangeRadioButton}
            >
              {sortBy.map((i) => (
                <FormControlLabel
                  key={i.name}
                  control={<Radio />}
                  label={i.name}
                  value={i.value}
                />
              ))}
            </RadioGroup>
          </FormControl>
        </AccordionDetails>
      </Accordion>
      <Accordion>
        <AccordionSummary
          expandIcon={<ExpandMoreIcon />}
          aria-controls='panel2bh-content'
          id='panel2bh-header'
        >
          <Typography>Brand</Typography>
        </AccordionSummary>
        <AccordionDetails>
          <FormGroup>
            {brands.map((i) => (
              <FormControlLabel
                key={i.id}
                control={
                  <Checkbox
                    value={i.id}
                    onChange={(e) => handleChangeCheckbox(e, 'brands')}
                  />
                }
                label={i.name}
              />
            ))}
          </FormGroup>
        </AccordionDetails>
      </Accordion>
      <Accordion>
        <AccordionSummary
          expandIcon={<ExpandMoreIcon />}
          aria-controls='panel3bh-content'
          id='panel3bh-header'
        >
          <Typography>Unlock Probability</Typography>
        </AccordionSummary>
        <AccordionDetails>
          <FormGroup>
            {unlockProbabilities.map((i) => (
              <FormControlLabel
                key={i.id}
                control={
                  <Checkbox
                    value={i.id}
                    onChange={(e) => handleChangeCheckbox(e, 'UnlockProbabilities')}
                  />
                }
                label={i.name}
              />
            ))}
          </FormGroup>
        </AccordionDetails>
      </Accordion>
      <Accordion>
        <AccordionSummary
          expandIcon={<ExpandMoreIcon />}
          aria-controls='panel4bh-content'
          id='panel4bh-header'
        >
          <Typography>Storage</Typography>
        </AccordionSummary>
        <AccordionDetails>
          <FormGroup>
            {storages.map((i) => (
              <FormControlLabel
                key={i}
                control={
                  <Checkbox
                    value={i}
                    onChange={(e) => handleChangeCheckbox(e, 'Storages')}
                  />
                }
                label={`${i} GB`}
              />
            ))}
          </FormGroup>
        </AccordionDetails>
      </Accordion>
      <Accordion>
        <AccordionSummary
          expandIcon={<ExpandMoreIcon />}
          aria-controls='panel4bh-content'
          id='panel5bh-header'
        >
          <Typography>Store</Typography>
        </AccordionSummary>
        <AccordionDetails>
          <FormGroup>
            {onlineStores.map((i) => (
              <FormControlLabel
                key={i.id}
                control={
                  <Checkbox
                    value={i.id}
                    onChange={(e) => handleChangeCheckbox(e, 'Stores')}
                  />
                }
                label={i.name}
              />
            ))}
          </FormGroup>
        </AccordionDetails>
      </Accordion>
      <Accordion>
        <AccordionSummary
          expandIcon={<ExpandMoreIcon />}
          aria-controls='panel4bh-content'
          id='panel6bh-header'
        >
          <Typography>Condition</Typography>
        </AccordionSummary>
        <AccordionDetails>
          <FormGroup>
            {conditions.map((i) => (
              <FormControlLabel
                key={i.id}
                control={
                  <Checkbox
                    value={i.id}
                    onChange={(e) => handleChangeCheckbox(e, 'Conditions')}
                  />
                }
                label={i.name}
              />
            ))}
          </FormGroup>
        </AccordionDetails>
      </Accordion>
      <Accordion>
        <AccordionSummary
          expandIcon={<ExpandMoreIcon />}
          aria-controls='panel4bh-content'
          id='panel7bh-header'
        >
          <Typography>Type</Typography>
        </AccordionSummary>
        <AccordionDetails>
          <FormGroup>
            {itemTypes.map((i) => (
              <FormControlLabel
                key={i.id}
                control={
                  <Checkbox
                    value={i.id}
                    onChange={(e) => handleChangeCheckbox(e, 'Types')}
                  />
                }
                label={i.name}
              />
            ))}
          </FormGroup>
        </AccordionDetails>
      </Accordion>
      <Accordion>
        <AccordionSummary
          expandIcon={<ExpandMoreIcon />}
          aria-controls='panel4bh-content'
          id='panel8bh-header'
        >
          <Typography>Price</Typography>
        </AccordionSummary>
        <AccordionDetails>
          <FormControl>
            <TextField
              id='minimum'
              label='Minimum'
              variant='outlined'
              sx={{ marginBottom: '10px' }}
              onChange={handleChangeMinimum}
            />
            <TextField
              id='maximum'
              label='Maximum'
              variant='outlined'
              onChange={handleChangeMaximum}
            />
          </FormControl>
        </AccordionDetails>
      </Accordion>
      <Accordion>
        <AccordionSummary
          expandIcon={<ExpandMoreIcon />}
          aria-controls='panel4bh-content'
          id='panel8bh-header'
        >
          <Typography>Ban keywords</Typography>
        </AccordionSummary>
        <AccordionDetails>
        <FormControl>
          <TextField
            id="Keyword"
            label="Keyword"
            variant="outlined"
            value={keyword} // Paso 2: Vincular el valor del input al estado
            onChange={handleBanKeywordChange} // Actualizar el estado cada vez que el input cambia
            sx={{ marginBottom: '10px' }}
          />
      <Button onClick={handleBanKeyword}>Ban</Button>
    </FormControl>
        </AccordionDetails>
      </Accordion>
    </div>
  );
};

export default Sidebar;

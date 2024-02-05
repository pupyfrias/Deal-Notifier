import AlertDialogSlide from '@/components/alert dialog slide/alert-dialog-slide';
import { useToast } from '@/hooks';
import { Item } from '@/models';
import { bulkDeleteItems } from '@/services';
import CancelIcon from '@mui/icons-material/Cancel';
import DeleteIcon from '@mui/icons-material/Delete';
import { Box, Button, Card, CardActions, CardContent, CardMedia, Checkbox, Fab, Typography } from '@mui/material';
import { ChangeEvent, FC, useEffect, useState } from 'react';
import { useLocation } from 'react-router-dom';

type HomeMainProps = {
  items: Item[];
  updateQueryParams: VoidFunction;
};

const HomeMain: FC<HomeMainProps> = ({ items, updateQueryParams }) => {
  const [checkboxesValue, setCheckboxesValue] = useState<number[]>([]);
  const [show, setShow] = useState(false);
  const location = useLocation();
  const toast = useToast();

  useEffect(() => {
    console.log('location.search changed');
  }, [location.search]);

  const handleCheckboxChange = (event: ChangeEvent<HTMLInputElement>, checked: boolean) => {
    const target = event.target;
    const value = parseInt(target.value);
    let newCheckboxesValue;

    if (checked) {
      newCheckboxesValue = [...checkboxesValue, value];
    } else {
      newCheckboxesValue = checkboxesValue.filter((c) => c !== value);
    }
    setCheckboxesValue(newCheckboxesValue);
  };

  const deleteItems = () => {
    setShow(true);
  };

  const onAgree = () => {
    console.log(checkboxesValue);
    bulkDeleteItems(checkboxesValue).then(() => {
      updateQueryParams();
      toast.showToast('Items deleted successfully');
    });

    unCheckAll();
  };

  const unCheckAll = () => {
    setCheckboxesValue([]);
  };

  const checkAll = (event: ChangeEvent<HTMLInputElement>, checked: boolean) => {
    if (checked) {
      const ids = items.map((item) => item.id);

      setCheckboxesValue(ids);
    } else {
      unCheckAll();
    }
  };

  return (
    <>
      <AlertDialogSlide
        title='Delete Items'
        contentText={`Are you sure to delete ${checkboxesValue.length} items?`}
        show={show}
        setShow={setShow}
        onAgree={onAgree}
      />
      {items.length > 0 ? (
        <div>
          {items.map((item) => (
            <Card
              key={item.id}
              sx={{ margin: '10px' }}
            >
              <CardContent sx={{ display: 'flex' }}>
                {item.image && (
                  <CardMedia
                    sx={{ height: 340, width: 250, margin: '10px' }}
                    image={item.image}
                  />
                )}
                <Box>
                  <a
                    href={item.link}
                    style={{ textDecoration: 'none', color: 'inherit' }}
                    target='blank'
                    rel='noopener noreferrer'
                  >
                    <Typography
                      gutterBottom
                      variant='h5'
                      component='div'
                    >
                      {item.name}
                    </Typography>
                  </a>

                  <Typography
                    variant='body1'
                    color='text.secondary'
                    component='span'
                  >
                    <p>
                      <strong>Price:</strong> {item.price}
                      {item.oldPrice > 0 && <span style={{ textDecoration: 'line-through', fontSize: '10px' }}>{item.oldPrice}</span>}
                    </p>

                    {item.saving > 0 && (
                      <p>
                        <strong>Saving: </strong>
                        {item.saving}
                      </p>
                    )}
                    <p>
                      <strong>Unlock Probability: </strong> {item.unlockProbabilityName}
                    </p>
                    <p>
                      <strong>Condition:</strong> {item.conditionName}
                    </p>
                    {item.isAuction && (
                      <p>
                        <strong>Auction</strong>
                      </p>
                    )}
                  </Typography>
                </Box>
                <Box sx={{ flexGrow: 1 }} />
                <Box>
                  <Checkbox
                    onChange={handleCheckboxChange}
                    value={item.id}
                    checked={checkboxesValue.includes(item.id)}
                  />
                </Box>
              </CardContent>
              <CardActions>
                <Button size='small'>Share</Button>
                <Button size='small'>Learn More</Button>
              </CardActions>
            </Card>
          ))}

          <Fab
            color='primary'
            aria-label='add'
            sx={{ position: 'fixed', right: '20px', bottom: '35px', zIndex: 'auto' }}
          >
            <Checkbox
              onChange={checkAll}
              style={{ color: 'white' }}
              checked={checkboxesValue.length == items.length}
            />
          </Fab>

          {checkboxesValue.length > 0 && (
            <Box
              sx={{
                width: '60%',
                height: '65px',
                boxShadow: '-2px 2px 14px -5px',
                borderRadius: '8px',
                margin: 'auto',
                display: 'flex',
                position: 'sticky',
                padding: '12px',
                alignItems: 'center',
                bottom: '30px',
                zIndex: 1,
                backgroundColor: 'white',
                marginTop: '40px'
              }}
            >
              <Typography>{checkboxesValue.length} Item(s) selected</Typography>
              <div style={{ flexGrow: 1 }}></div>

              <Button
                variant='contained'
                startIcon={<DeleteIcon />}
                onClick={deleteItems}
                color='error'
                sx={{ marginRight: '5px' }}
              >
                Delete
              </Button>
              <Button
                variant='contained'
                startIcon={<CancelIcon />}
                onClick={unCheckAll}
              >
                Cancel
              </Button>
            </Box>
          )}
        </div>
      ) : (
        <Typography variant='h4'>Items not found</Typography>
      )}
    </>
  );
};

export default HomeMain;

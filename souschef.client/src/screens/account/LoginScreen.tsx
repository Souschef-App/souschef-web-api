import React from 'react';
import {StyleSheet, Text} from 'react-native';
import MaterialCommunityIcon from 'react-native-vector-icons/MaterialCommunityIcons';
import {Button, Card, Column, Input, Row, SafeArea} from '../../components';
import {OpacityPressable, SpringPressable} from '../../components/pressable';
import {LoginScreenNavigationProp} from '../../navigation/types';
import {theme} from '../../styles/theme';

const LoginScreen = ({navigation, route}: LoginScreenNavigationProp) => {
  const [email, onChangeEmail] = React.useState('');
  const [password, onChangePassword] = React.useState('');
  const [error, setError] = React.useState('');

  const login = () => {
    setError('');
    // Empty fields
    if (email.length === 0 || password.length === 0) {
      setError('Please make sure all fields are filled.');
    }
    // Successfully log'd in
    else {
      navigation.navigate('Home');
    }
  };

  const register = () => {
    navigation.navigate('Register');
  };

  return (
    <SafeArea>
      <Column horizontalResizing="fill" verticalResizing="fill">
        <Column horizontalResizing="fill">
          <Text style={styles.h1}>Hello Again!</Text>
          <Text style={styles.h2}>Welcome back, you've been missed!</Text>
          {error.length > 0 && (
            <Card style={styles.error}>
              <Column horizontalResizing="fill">
                <MaterialCommunityIcon name="cancel" style={styles.errorIcon} />
                <Text style={styles.errorText}>{error}</Text>
              </Column>
            </Card>
          )}
        </Column>
        <Column horizontalResizing="fill" style={{marginVertical: 32}}>
          <Input
            placeholder="Email"
            horizontalResizing="fill"
            onChangeText={value => {
              onChangeEmail(value);
            }}
          />
          <Input
            placeholder="Password"
            secure={true}
            horizontalResizing="fill"
            onChangeText={value => {
              onChangePassword(value);
            }}
            style={{marginTop: 8}}
          />
        </Column>
        <SpringPressable onPress={login} horizontalResizing="fill">
          <Button
            bgColor={theme.colors.red}
            horizontalResizing="fill"
            verticalResizing="fixed"
            height={64}
            text="Login"
            textStyle={styles.buttonText}
          />
        </SpringPressable>
        <Row paddingVertical={16}>
          <Text style={styles.registerText}>Not a member?</Text>
          <OpacityPressable onPress={register}>
            <Text style={[styles.registerText, styles.clickableText]}>
              Register
            </Text>
          </OpacityPressable>
        </Row>
      </Column>
    </SafeArea>
  );
};

const styles = StyleSheet.create({
  h1: {
    color: theme.colors.lightText,
    fontSize: 28,
    fontWeight: 'bold',
    alignSelf: 'stretch',
    textAlign: 'center',
  },
  h2: {
    color: theme.colors.lightText,
    fontSize: 18,
    alignSelf: 'stretch',
    textAlign: 'center',
    marginTop: theme.spacing.s,
  },
  error: {
    backgroundColor: theme.colors.blue,
    marginTop: theme.spacing.l,
    elevation: 0,
  },
  errorText: {
    color: '#fff',
    fontSize: 16,
    marginTop: theme.spacing.s,
  },
  errorIcon: {color: '#fff', fontSize: 36},
  buttonText: {
    fontSize: 20,
    fontWeight: 'bold',
  },
  registerText: {
    color: theme.colors.lightText,
    fontSize: 16,
  },
  clickableText: {
    color: '#2A60A6',
    marginLeft: 8,
  },
});

export default LoginScreen;

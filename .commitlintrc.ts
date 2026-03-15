import { RuleConfigSeverity } from '@commitlint/types';

const Configuration = {
  /*
   * Resolve and load @commitlint/config-conventional from node_modules.
   * Referenced packages must be installed
   */
  extends: ['@commitlint/config-conventional'],
  /*
   * Resolve and load conventional-changelog-conventionalcommits from node_modules.
   * Referenced packages must be installed
   */
  parserPreset: 'conventional-changelog-conventionalcommits',
  /*
   * Resolve and load @commitlint/format from node_modules.
   * Referenced package must be installed
   */
  formatter: '@commitlint/format',
  /*
   * Any rules defined here will override rules from @commitlint/config-conventional
   */
  rules: {
    'body-max-line-length': [RuleConfigSeverity.Disabled],
  },
  /*
   * Array of functions that return true if commitlint should ignore the given message.
   * Given array is merged with predefined functions, which consist of matchers like:
   *
   * - 'Merge pull request', 'Merge X into Y' or 'Merge branch X'
   * - 'Revert X'
   * - 'v1.2.3' (ie semver matcher)
   * - 'Automatic merge X' or 'Auto-merged X into Y'
   *
   * To see full list, check https://github.com/conventional-changelog/commitlint/blob/master/%40commitlint/is-ignored/src/defaults.ts.
   * To disable those ignores and run rules always, set `defaultIgnores: false` as shown below.
   */
  ignores: [(message: string) =>
    message === '' ||
    // Ignore commits from dependabot and renovate
    message.includes('Signed-off-by: dependabot[bot]') ||
    message.startsWith('build(deps): bump') ||
    message.startsWith('Bump the')],
  /*
   * Whether commitlint uses the default ignore rules, see the description above.
   */
  defaultIgnores: true,
};
export default Configuration;
